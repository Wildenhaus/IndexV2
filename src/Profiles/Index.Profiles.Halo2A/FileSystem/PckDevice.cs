using Index.Core.IO;
using Index.Domain.FileSystem;
using Index.Profiles.Halo2A.FileSystem.Files;
using LibSaber.Halo2A.IO;

namespace Index.Profiles.Halo2A.FileSystem
{

  public class PckDevice : FileSystemDeviceBase
  {

    #region Data Members

    private readonly string _filePath;
    private H2AStreamCompressionInfo _compressionInfo;

    #endregion

    #region Constructor

    public PckDevice( string filePath )
    {
      _filePath = filePath;
    }

    #endregion

    #region Overrides

    public override Stream GetStream( IFileSystemNode node )
    {
      var saberNode = node as H2AFileSystemNode;
      ASSERT( saberNode != null, "Node is not a SaberFileSystemNode." );

      var startOffset = saberNode.StartOffset;
      var sizeInBytes = saberNode.SizeInBytes;
      var stream = CreateStream();

      return new StreamSegment( stream, startOffset, sizeInBytes );
    }

    protected override Task<IResult<IFileSystemNode>> OnInitializing( CancellationToken cancellationToken = default )
    {
      return Task.Run( () =>
      {
        var rootNode = InitNodes();
        return ( IResult<IFileSystemNode> ) Result.Successful( rootNode );
      } );
    }

    #endregion

    #region Private Methods

    private Stream CreateStream()
    {
      var stream = H2AStream.FromFile( _filePath, _compressionInfo );
      if ( _compressionInfo.UncompressedLength == 0 )
        _compressionInfo = stream.CompressionInfo;

      return stream;
    }

    private IFileSystemNode InitNodes()
    {
      // Initialize Root Node
      var fileName = Path.GetFileNameWithoutExtension( _filePath );
      var rootNode = new H2AFileSystemNode( this, fileName );

      // Initialize Entries
      var reader = new NativeReader( CreateStream(), Endianness.LittleEndian );

      ReadHeader( reader );
      ReadChildren( reader, rootNode );

      return rootNode;
    }

    private void ReadHeader( NativeReader reader )
    {
      // TODO: Skipping this for now
      reader.Position += 0x45;
    }

    private void ReadChildren( NativeReader reader, IFileSystemNode rootNode, long additionalOffset = 0 )
    {
      var entryCount = reader.ReadInt32();
      _ = reader.ReadInt32(); // TODO

      _ = reader.ReadByte(); // Delimiter

      // Entry Names
      var names = new string[ entryCount ];
      for ( var i = 0; i < entryCount; i++ )
        names[ i ] = reader.ReadPascalString32();

      _ = reader.ReadByte(); // Delimiter

      // Entry Offsets
      var offsets = new long[ entryCount ];
      for ( var i = 0; i < entryCount; i++ )
        offsets[ i ] = reader.ReadInt64();

      _ = reader.ReadByte(); // Delimiter

      // Sizes
      var sizes = new int[ entryCount ];
      for ( var i = 0; i < entryCount; i++ )
        sizes[ i ] = reader.ReadInt32();

      // Create entries
      for ( var i = 0; i < entryCount; i++ )
      {
        var name = names[ i ];
        var offset = offsets[ i ];
        var size = sizes[ i ];

        if ( size == 0 )
          continue;

        CreateNode( reader, name, offset + additionalOffset, size, rootNode );
      }
    }

    private void CreateNode( NativeReader reader, string name, long offset, int size, IFileSystemNode parent )
    {
      name = SanitizeName( name );
      if ( IsNestedContainer( reader, name, offset, size ) )
        CreateNodeForNestedContainer( reader, offset, parent );
      else
        CreateNodeForFileEntry( name, offset, size, parent );
    }

    private bool IsNestedContainer( NativeReader reader, string name, long offset, int size )
    {
      const long MAGIC_1SERpak = 0x006B617052455331;

      reader.Position = offset;
      var signature = reader.ReadInt64();
      reader.Position -= sizeof( long );

      return signature == MAGIC_1SERpak;
    }

    private void CreateNodeForNestedContainer( NativeReader reader, long offset, IFileSystemNode parent )
    {
      ReadHeader( reader );
      ReadChildren( reader, parent, offset );
    }

    private void CreateNodeForFileEntry( string name, long offset, int size, IFileSystemNode parent )
    {
      H2AFileSystemNode node;

      switch ( Path.GetExtension( name ) )
      {
        case ".pct":
          node = new H2ATextureFileNode( this, name, offset, size, parent );
          break;
        case ".lg":
          node = new H2ASceneFileNode( this, name, offset, size, parent );
          break;
        case ".tpl":
          node = new H2ATemplateFileNode( this, name, offset, size, parent );
          break;

        default:
          node = new H2AFileSystemNode( this, name, offset, size, parent );
          break;
      }

      parent.AddChild( node );
    }

    private static string SanitizeName( string name )
    {
      if ( name.Contains( ":" ) )
        name = name.Substring( name.IndexOf( ':' ) + 1 );
      if ( name.Contains( ">" ) )
        name = name.Substring( name.IndexOf( '>' ) + 1 );

      return Path.GetFileName( name );
    }

    #endregion

  }
}
