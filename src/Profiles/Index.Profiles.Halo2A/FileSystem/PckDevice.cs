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
      CreateContainerBlockNodes( reader, rootNode );

      return rootNode;
    }

    private void CreateContainerBlockNodes( NativeReader reader, IFileSystemNode parent, long additionalOffset = 0 )
    {
      if ( !TryReadBlockMagic( reader, out var blockSignature ) )
        return;

      // Skip Header
      reader.Position += 0x45;

      var entries = ReadBlockEntries( reader, additionalOffset );
      foreach ( var entry in entries )
        CreateNode( reader, entry.Name, entry.Offset, entry.SizeInBytes, parent );
    }

    private void CreateNode( NativeReader reader, string name, long offset, int size, IFileSystemNode parent )
    {
      name = SanitizeName( name );
      reader.Position = offset;

      if ( IsNestedContainer( reader, name, offset, size ) )
        CreateNodeForNestedContainer( reader, name, offset, parent );
      else
        CreateNodeForFileEntry( name, offset, size, parent );
    }

    private bool IsNestedContainer( NativeReader reader, string name, long offset, int size )
    {
      if ( !TryReadBlockMagic( reader, out var signature ) )
        return false;

      switch ( signature )
      {
        case "1SERpak":
        case "1SERcache_block":
          return true;

        default:
          return false;
      }
    }

    private void CreateNodeForNestedContainer( NativeReader reader, string name, long offset, IFileSystemNode parent )
    {
      var containerNode = new H2AFileSystemNode( this, name, parent );
      parent.AddChild( containerNode );

      CreateContainerBlockNodes( reader, containerNode, offset );
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
        case ".td":
          node = new H2ATextureDefinitionFileNode( this, name, offset, size, parent );
          break;

        case ".dsh":
        case ".fx":
        case ".hsh":
        case ".psh":
        case ".vsh":
          node = new H2AShaderCodeFileNode( this, name, offset, size, parent );
          break;

        default:
          node = new H2AFileSystemNode( this, name, offset, size, parent );
          break;
      }

      parent.AddChild( node );
    }

    private bool TryReadBlockMagic( NativeReader reader, out string signature )
    {
      const uint MAGIC_1SER = 0x52455331; // 1SER

      signature = default;
      var originalOffset = reader.Position;

      var magic = reader.ReadUInt32();
      reader.Position = originalOffset;

      if ( magic != MAGIC_1SER )
        return false;

      signature = reader.ReadNullTerminatedString();
      reader.Position = originalOffset;
      return true;
    }

    private PakEntry[] ReadBlockEntries( NativeReader reader, long additionalOffset = 0 )
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
      var entries = new List<PakEntry>( entryCount );
      for ( var i = 0; i < entryCount; i++ )
      {
        var name = names[ i ];
        var offset = offsets[ i ] + additionalOffset;
        var size = sizes[ i ];

        if ( size == 0 )
          continue;

        entries.Add( new PakEntry( name, offset, size ) );
      }

      return entries.ToArray();
    }

    private static string SanitizeName( string name )
    {
      if ( name.Contains( ":" ) )
        name = name.Substring( name.IndexOf( ':' ) + 1 );
      if ( name.Contains( ">" ) )
        name = name.Substring( name.IndexOf( '>' ) + 1 );

      return name;
    }

    #endregion

    #region Embedded Types

    private readonly struct PakEntry
    {

      #region Data Members

      public readonly string Name;
      public readonly long Offset;
      public readonly int SizeInBytes;

      #endregion

      #region Constructor

      public PakEntry( string name, long offset, int sizeInBytes )
      {
        Name = name;
        Offset = offset;
        SizeInBytes = sizeInBytes;
      }

      #endregion

    }

    #endregion

  }
}
