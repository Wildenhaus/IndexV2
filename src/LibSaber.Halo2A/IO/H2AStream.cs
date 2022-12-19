using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSaber.IO;
using static LibSaber.Assertions;

namespace LibSaber.Halo2A.IO
{

  public class H2AStream : Stream
  {

    #region Constants

    private const long HEADER_SIZE = 0x600000;
    private const long MAX_CHUNK_SIZE = 0x8000;
    private const long MAX_CHUNK_COUNT = ( HEADER_SIZE - sizeof( long ) ) / sizeof( long );

    #endregion

    #region Data Members

    private readonly Stream _baseStream;
    private readonly NativeReader _reader;

    private H2AStreamCompressionInfo _compressionInfo;

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

    public H2AStreamCompressionInfo CompressionInfo => _compressionInfo;

    private bool IsCompressed => CompressionInfo.IsCompressed;
    private int ChunkCount => CompressionInfo.ChunkCount;
    private H2AStreamChunk CurrentChunk => Chunks[ _currentChunkIndex ];
    private H2AStreamChunk[] Chunks => CompressionInfo.Chunks;

    #endregion

    #region Constructor

    private H2AStream( Stream baseStream, H2AStreamCompressionInfo compressionInfo = default )
    {
      _baseStream = baseStream;
      _compressionInfo = compressionInfo;

      _reader = new NativeReader( baseStream, Endianness.LittleEndian );

      _decompressBuffer = new byte[ MAX_CHUNK_SIZE ];
      _decompressStream = new MemoryStream( _decompressBuffer );
    }

    public static H2AStream FromStream( Stream baseStream, H2AStreamCompressionInfo compressionInfo = default )
    {
      var stream = new H2AStream( baseStream, compressionInfo );
      stream.Initialize();

      return stream;
    }

    public static H2AStream FromFile( string filePath, H2AStreamCompressionInfo compressionInfo = default )
      => FromStream( File.OpenRead( filePath ), compressionInfo );

    #endregion

    #region Overrides

    public override int Read( byte[] buffer, int offset, int size )
    {
      if ( size == 0 )
        return -1;

      if ( IsCompressed )
        return ReadCompressed( buffer, offset, size );
      else
        return ReadUncompressed( buffer, offset, size );
    }

    public override long Seek( long offset, SeekOrigin origin )
    {
      // Calculate the true offset.
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

      ASSERT( offset >= 0, "Seek offset must be positive." );
      ASSERT( offset < Length, "Seek offset was out of bounds." );

      // Find the appropriate chunk.
      var chunkIndex = ( int ) ( offset / MAX_CHUNK_SIZE );
      SetCurrentChunk( chunkIndex );

      // Set position within chunk
      var chunkPosition = offset - _position;

      if ( IsCompressed )
        _decompressStream.Position = chunkPosition;
      else
        _baseStream.Position += chunkPosition;

      _position += chunkPosition;

      ASSERT( _position == offset );
      return _position;
    }

    public override void Write( byte[] buffer, int offset, int count )
    {
      FAIL( "Stream does not support modification." );
    }

    public override void SetLength( long value )
    {
      FAIL( "Stream does not support modification." );
    }

    public override void Flush()
    {
      FAIL( "Stream does not support modification." );
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
      if ( _compressionInfo.UncompressedLength == 0 )
        InitializeCompressionInfo();

      _position = 0;
      SetCurrentChunk( 0 );
    }

    private void InitializeCompressionInfo()
    {
      var reader = _reader;
      reader.Position = 0;

      // Read Chunk Count/Compression Type
      var chunkCount = reader.ReadInt32();
      var isCompressed = reader.ReadInt32() == 0;

      // Read Chunk Table
      var offsets = new long[ chunkCount ];
      for ( var i = 0; i < chunkCount; i++ )
        offsets[ i ] = reader.ReadInt64();

      // Create Chunks
      var chunks = new H2AStreamChunk[ chunkCount ];
      var streamLength = _baseStream.Length;
      var lastChunkOffset = offsets[ offsets.Length - 1 ];
      for ( var i = 0; i < chunkCount; i++ )
      {
        var startOffset = offsets[ i ];
        long endOffset = startOffset;

        if ( i < chunkCount - 1 )
          endOffset = offsets[ i + 1 ];
        else
          endOffset = Math.Min( startOffset + MAX_CHUNK_SIZE, streamLength );

        var uncompressedLength = MAX_CHUNK_SIZE;

        // If the stream is compressed, we need to obtain the
        // uncompressed size of the final chunk.
        if ( i == offsets.Length - 1 && isCompressed )
        {
          _baseStream.Position = lastChunkOffset;
          using ( var zlibStream = new ZLibStream( _baseStream, CompressionMode.Decompress, true ) )
          {
            var tmpStream = new MemoryStream();
            zlibStream.CopyTo( tmpStream );
            uncompressedLength = tmpStream.Length;
          }
        }

        chunks[ i ] = new H2AStreamChunk( startOffset, endOffset, uncompressedLength );
      }

      _compressionInfo = new H2AStreamCompressionInfo( isCompressed, chunks );
    }

    private void IncrementCurrentChunk()
      => SetCurrentChunk( _currentChunkIndex + 1 );

    private void SetCurrentChunk( int chunkIndex )
    {
      ASSERT( chunkIndex >= 0 && chunkIndex < ChunkCount, "Chunk index out of bounds." );

      var chunk = Chunks[ chunkIndex ];

      _baseStream.Position = chunk.StartOffset;
      _currentChunkIndex = chunkIndex;

      if ( IsCompressed )
        DecompressChunk( chunkIndex );

      _position = chunkIndex * MAX_CHUNK_SIZE;
    }

    private void DecompressChunk( int chunkIndex )
    {
      var chunk = Chunks[ chunkIndex ];
      _baseStream.Position = chunk.StartOffset;

      using ( var zlibStream = new ZLibStream( _baseStream, CompressionMode.Decompress, true ) )
      {
        _decompressStream.SetLength( MAX_CHUNK_SIZE );
        long bytesRead = 0;
        while ( bytesRead < chunk.UncompressedLength )
          bytesRead += zlibStream.Read( _decompressBuffer, ( int ) bytesRead, ( int ) ( chunk.UncompressedLength - bytesRead ) );

        _decompressStream.Position = 0;
        _decompressStream.SetLength( bytesRead );
      }
    }

    private int ReadUncompressed( byte[] buffer, int offset, int size )
    {
      var bytesRead = 0;

      while ( bytesRead < size )
      {
        if ( _position >= Length )
          return bytesRead;

        var remainingChunkBytes = CurrentChunk.EndOffset - _baseStream.Position;
        if ( remainingChunkBytes <= 0 )
        {
          IncrementCurrentChunk();
          remainingChunkBytes = CurrentChunk.EndOffset - _baseStream.Position;
        }

        var bytesToRead = ( int ) Math.Min( size - bytesRead, remainingChunkBytes );
        _baseStream.Read( buffer, bytesRead, bytesToRead );

        bytesRead += bytesToRead;
        _position += bytesToRead;
      }

      return bytesRead;
    }

    private int ReadCompressed( byte[] buffer, int offset, int size )
    {
      var bytesRead = 0;

      while ( bytesRead < size )
      {
        if ( _position == Length )
          return bytesRead;

        var remainingChunkBytes = _decompressStream.Length - _decompressStream.Position;
        if ( remainingChunkBytes <= 0 )
        {
          IncrementCurrentChunk();
          remainingChunkBytes = _decompressStream.Length - _decompressStream.Position;
        }

        var bytesToRead = ( int ) Math.Min( size - bytesRead, remainingChunkBytes );

        _decompressStream.Read( buffer, bytesRead, bytesToRead );
        bytesRead += bytesToRead;
        _position += bytesToRead;
      }

      return bytesRead;
    }

    #endregion

  }

}