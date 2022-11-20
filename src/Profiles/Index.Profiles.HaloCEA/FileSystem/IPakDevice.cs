using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Core.IO;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.FileSystem.Files;
using Index.Profiles.HaloCEA.IO;

namespace Index.Profiles.HaloCEA.FileSystem
{

  public class IPakDevice : FileSystemDeviceBase
  {

    #region Constants

    private const int TEXTURE_NAME_LENGTH = 0x100;
    private const long TEXTURE_DATA_OFFSET = 0x290008;

    #endregion

    #region Data Members

    private readonly string _filePath;
    private CEAStreamCompressionInfo _streamCompressionInfo;

    #endregion

    #region Constructor

    public IPakDevice( string filePath )
    {
      _filePath = filePath;
    }

    #endregion

    #region Overrides

    protected override Task<IResult<IFileSystemNode>> OnInitializing( CancellationToken cancellationToken = default )
    {
      return Task.Factory.StartNew( () =>
      {
        var rootNode = InitNodes();
        return ( IResult<IFileSystemNode> ) Result.Successful( rootNode );
      } );
    }

    public override Stream GetStream( IFileSystemNode node )
    {
      var ceaNode = node as CEATextureFileNode;
      ASSERT( ceaNode is not null, "File node is not a CEAFileNode" );

      var stream = CreateStream();
      var startOffset = ceaNode.StartOffset;
      var fileSize = ceaNode.SizeInBytes;

      return new StreamSegment( stream, startOffset, fileSize );
    }

    #endregion

    #region Private Methods

    private Stream CreateStream()
    {
      var stream = CEAStream.FromFile( _filePath, _streamCompressionInfo );
      if ( _streamCompressionInfo.UncompressedLength == 0 )
        _streamCompressionInfo = stream.CompressionInfo;

      return stream;
    }

    private IFileSystemNode InitNodes()
    {
      // Initialize Root Node
      var pakName = Path.GetFileName( _filePath );
      var rootNode = new CEAFileNode( this, pakName );

      // Initialize Entries
      var reader = new NativeReader( CreateStream(), Endianness.LittleEndian );

      var fileCount = reader.ReadInt32();
      var unk_00 = reader.ReadInt32();

      for ( var i = 0; i < fileCount; i++ )
        CreateFileNode( reader, rootNode );

      return rootNode;
    }

    private void CreateFileNode( NativeReader reader, IFileSystemNode parent )
    {
      // The texture's file name is always 0x100 bytes long.
      var fileName = reader.ReadFixedLengthString( TEXTURE_NAME_LENGTH );
      _ = reader.ReadInt64();
      _ = reader.ReadInt32();

      var width = reader.ReadInt32();
      var height = reader.ReadInt32();
      var depth = reader.ReadInt32();
      var mipCount = reader.ReadInt32();
      var faceCount = reader.ReadInt32();
      var format = ( CEATextureFormat ) reader.ReadInt32();
      _ = reader.ReadInt64();

      // This part is weird.
      // The size of the raw texture data is stored three times.
      // We're reading all of them so we can assert their equality.
      var fileSize = reader.ReadInt32();
      _ = reader.ReadUInt32();

      var fileSize_B = reader.ReadInt32();
      var startOffset = reader.ReadInt32();
      _ = reader.ReadInt32();

      var fileSize_C = reader.ReadInt32();
      _ = reader.ReadInt32();

      ASSERT( fileSize == fileSize_B, "Texture data size mismatch." );
      ASSERT( fileSize == fileSize_C, "Texture data size mismatch." );
      ASSERT( startOffset >= TEXTURE_DATA_OFFSET,
        "Texture data start offset does not fall within correct bounds." );

      var node = new CEATextureFileNode( this, fileName, parent )
      {
        StartOffset = startOffset,
        SizeInBytes = fileSize,

        Width = width,
        Height = height,
        Depth = depth,
        MipCount = mipCount,
        FaceCount = faceCount,
        Format = format,
      };

      parent.AddChild( node );
    }

    #endregion

  }

}
