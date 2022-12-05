using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSaber.IO;

namespace LibSaber.HaloCEA.IO
{

  public class CEAStream : Stream
  {

    /* ========================================================================
     * Halo CEA streams are usually ZLib compressed and have a fixed-length
     * header. The header layout is as follows:
     * 
     *   0x00000: int ChunkCount
     *   0x00004: Start of chunk table
     *   0x40000: First chunk of data
     * 
     * The chunk table is an int[ChunkCount], where each element is the chunk offset.
     * This table is always (0x40000 - 4) bytes long, and any leftover space after the
     * chunk offsets will be zero data.
     *   
     * At 0x40000, the chunk data begins. Each chunk is prefixed by the uncompressed
     * size of the chunk (int). It does not supply the compressed size, so we calculate that
     * based off of the offsets (factoring in the 4 bytes denoting the uncompressed size
     * at the beginning of the chunk).
     */

    #region Constants

    /// <summary>
    ///   The constant size of a stream's header.
    /// </summary>
    private const int HEADER_SIZE = 0x40000;

    /// <summary>
    ///   The maximum number of chunks a stream can have.
    /// </summary>
    /// <remarks>
    ///   We take the HEADER_SIZE and subtract sizeof( int ) to get the header size minus
    ///   the 
    /// </remarks>
    private const int MAX_CHUNK_COUNT = ( HEADER_SIZE - sizeof( int ) ) / sizeof( int );

    /// <summary>
    ///   The maximum size of a chunk.
    /// </summary>
    private const int MAX_CHUNK_SIZE = 0x20000;

    #endregion

    #region Data Members

    private readonly Stream _baseStream;
    private readonly NativeReader _reader;

    private CEAStreamCompressionInfo _compressionInfo;

    private long _position;
    private int _currentChunkIndex;

    private byte[] _decompressBuffer;
    private MemoryStream _decompressStream;

    #endregion

    #region Properties

    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => false;

    public override long Length => _compressionInfo.UncompressedLength;

    public override long Position
    {
      get => _position;
      set => Seek( value, SeekOrigin.Begin );
    }

    public CEAStreamCompressionInfo CompressionInfo => _compressionInfo;

    private int ChunkCount => CompressionInfo.ChunkCount;
    private CEAStreamChunk CurrentChunk => Chunks[ _currentChunkIndex ];
    private CEAStreamChunk[] Chunks => CompressionInfo.Chunks;

    #endregion

    #region Constructor

    private CEAStream( Stream baseStream, CEAStreamCompressionInfo compressionInfo = default )
    {
      _baseStream = baseStream;
      _compressionInfo = compressionInfo;

      _reader = new NativeReader( baseStream, Endianness.LittleEndian );

      _decompressBuffer = new byte[ MAX_CHUNK_SIZE ];
      _decompressStream = new MemoryStream( _decompressBuffer );
    }

    public static CEAStream FromStream( Stream baseStream, CEAStreamCompressionInfo compressionInfo = default )
    {
      var stream = new CEAStream( baseStream, compressionInfo );
      stream.Initialize();

      return stream;
    }

    public static CEAStream FromFile( string filePath, CEAStreamCompressionInfo compressionInfo = default )
      => CEAStream.FromStream( File.OpenRead( filePath ), compressionInfo );

    #endregion

    #region Overrides

    public override int Read( byte[] buffer, int offset, int count )
    {
      if ( count == 0 )
        return -1;

      var bytesRead = 0;
      while ( bytesRead < count )
      {
        if ( Position == Length )
          return bytesRead;

        var remainingChunkBytes = _decompressStream.Length - _decompressStream.Position;
        if ( remainingChunkBytes <= 0 )
        {
          IncrementCurrentChunk();
          remainingChunkBytes = CurrentChunk.UncompressedLength - _decompressStream.Position;
        }

        var bytesToRead = ( int ) Math.Min( count - bytesRead, remainingChunkBytes );

        _decompressStream.Read( buffer, bytesRead, bytesToRead );
        bytesRead += bytesToRead;
        _position += bytesToRead;
      }

      return bytesRead;
    }

    public override long Seek( long offset, SeekOrigin origin )
    {
      // Calculate the true offset
      switch ( origin )
      {
        case SeekOrigin.Begin:
          break;
        case SeekOrigin.End:
          offset = offset + Length;
          break;
        case SeekOrigin.Current:
          offset = offset + _position;
          break;
      }

      if ( offset == _position )
        return _position;

      ASSERT( offset >= 0 && offset <= Length, "Seek offset falls out of bounds." );

      var chunkIndex = ( int ) ( offset / MAX_CHUNK_SIZE );
      if ( chunkIndex != _currentChunkIndex )
        SetCurrentChunk( chunkIndex );

      // Set position within chunk
      var chunkPosition = offset % MAX_CHUNK_SIZE;
      _decompressStream.Position = chunkPosition;
      _position = offset;

      return _position;
    }

    public override void Write( byte[] buffer, int offset, int count )
    {
      throw new NotSupportedException();
    }

    public override void SetLength( long value )
    {
      throw new NotSupportedException();
    }

    public override void Flush()
    {
      throw new NotSupportedException();
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
      if ( !_compressionInfo.IsInitialized )
        InitializeCompressionInfo();

      _position = 0;
      SetCurrentChunk( 0 );
    }

    private void InitializeCompressionInfo()
    {
      var reader = _reader;
      reader.Position = 0;

      // Read Chunk Count at 0x0
      var chunkCount = reader.ReadInt32();
      ASSERT( chunkCount > 0, "Chunk Count must be a positive value." );
      ASSERT( chunkCount <= MAX_CHUNK_COUNT, "Stream exceeds the maximum chunk count ({0}).", MAX_CHUNK_COUNT );

      // Read Chunk Table
      var offsets = new long[ chunkCount ];
      for ( var i = 0; i < chunkCount; i++ )
        offsets[ i ] = reader.ReadInt32();

      // Create Chunks
      var chunks = new CEAStreamChunk[ chunkCount ];
      var streamLength = _baseStream.Length;
      for ( var i = 0; i < chunkCount; i++ )
      {
        // First 4 bytes of the chunk is the size.
        reader.Position = offsets[ i ];
        var uncompressedLength = reader.ReadInt32();

        var startOffset = reader.Position;
        var endOffset = i == chunkCount - 1 ? streamLength : offsets[ i + 1 ];

        chunks[ i ] = new CEAStreamChunk( startOffset, endOffset, uncompressedLength );
      }

      _compressionInfo = new CEAStreamCompressionInfo( chunks );
    }

    private void IncrementCurrentChunk()
      => SetCurrentChunk( _currentChunkIndex + 1 );

    private void SetCurrentChunk( int chunkIndex )
    {
      ASSERT( chunkIndex >= 0 && chunkIndex < ChunkCount, "Chunk index out of bounds." );

      var chunk = Chunks[ chunkIndex ];

      _baseStream.Position = chunk.StartOffset;
      _currentChunkIndex = chunkIndex;

      DecompressChunk( chunkIndex );
      _position = chunkIndex * MAX_CHUNK_SIZE;
    }

    private void DecompressChunk( int chunkIndex )
    {
      var chunk = Chunks[ chunkIndex ];
      var uncompressedLength = chunk.UncompressedLength;

      _baseStream.Position = chunk.StartOffset;

      using ( var zlibStream = new ZLibStream( _baseStream, CompressionMode.Decompress, true ) )
      {
        _decompressStream.SetLength( uncompressedLength );

        var totalBytesRead = 0;
        var bytesRemaining = uncompressedLength;

        while ( totalBytesRead < uncompressedLength )
        {
          var bytesRead = zlibStream.Read( _decompressBuffer, totalBytesRead, ( int ) bytesRemaining );
          if ( bytesRead == 0 )
            break;

          totalBytesRead += bytesRead;
          bytesRemaining -= bytesRead;
        }

        _decompressStream.Position = 0;
        _decompressStream.SetLength( totalBytesRead );
      }
    }

    #endregion

  }

}
