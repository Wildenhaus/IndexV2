using Index.Core.IO;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.FileSystem.Files;
using LibSaber.HaloCEA.IO;

namespace Index.Profiles.HaloCEA.FileSystem
{

  public class S3DPakDevice : FileSystemDeviceBase
  {

    #region Data Members

    private readonly string _filePath;
    private CEAStreamCompressionInfo _streamCompressionInfo;

    #endregion

    #region Constructor

    public S3DPakDevice( string filePath )
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
      var ceaNode = node as CEAFileNode;
      ASSERT( ceaNode is not null, "File node is not a CEAFileNode." );

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
      var pakName = Path.GetFileNameWithoutExtension( _filePath );
      var rootNode = new CEAFileNode( this, pakName );

      // Initialize Entries
      var reader = new NativeReader( CreateStream(), Endianness.LittleEndian );

      var fileCount = reader.ReadInt32();
      for ( var i = 0; i < fileCount; i++ )
        InitFileNode( reader, rootNode );

      return rootNode;
    }

    private void InitFileNode( NativeReader reader, IFileSystemNode parent )
    {
      var startOffset = reader.ReadInt32();
      var fileSize = reader.ReadInt32();
      var fileName = reader.ReadPascalString32();
      var fileType = ( CEAFileType ) reader.ReadInt32();
      var unk_00 = reader.ReadInt64(); // TODO: Figure out what this is

      var node = CreateTypedFileNode( fileType, fileName, parent );
      node.StartOffset = startOffset;
      node.SizeInBytes = fileSize;
      node.FileType = fileType;


      parent.AddChild( node );
    }

    private CEAFileNode CreateTypedFileNode( CEAFileType fileType, string fileName, IFileSystemNode parent )
    {
      switch ( fileType )
      {
        case CEAFileType.Template:
          return new CEATemplateFileNode( this, fileName, parent );
        case CEAFileType.Scene:
          return new CEASceneFileNode( this, fileName, parent );

        default:
          return new CEAFileNode( this, fileName, parent );
      }
    }

    #endregion

  }

}
