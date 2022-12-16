using System.Text;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.FileSystem.Files;
using Index.Utilities;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;

namespace Index.Profiles.HaloCEA.FileSystem
{

  public class CacheBlockDevice : FileSystemDeviceBase
  {

    #region Data Members

    private readonly IReadOnlyList<IFileSystemDevice> _devices;

    #endregion

    #region Constructor

    public CacheBlockDevice( IReadOnlyList<IFileSystemDevice> devices )
    {
      _devices = devices;
    }

    #endregion

    #region Overrides

    public override Stream GetStream( IFileSystemNode node )
    {
      var cacheBlockEntryNode = node as CEACacheBlockEntryFileNode;
      if ( node is null )
        FAIL( "Node is not a CEACacheBlockEntryFileNode" );

      return new MemoryStream( Encoding.UTF8.GetBytes( cacheBlockEntryNode.EntryData ) );
    }

    protected override async Task<IResult<IFileSystemNode>> OnInitializing( CancellationToken cancellationToken = default )
    {
      var rootNode = new CEAFileNode( this, "root" );
      var fileNameLookup = CreateFileNameHashLookup();

      foreach ( var device in _devices )
        foreach ( var node in device.EnumerateFiles().OfType<CEACacheBlockFileNode>() )
          rootNode.AddChild( CreateCacheBlockEntryNodes( rootNode, node, fileNameLookup ) );

      return Result.Successful( rootNode );
    }

    #endregion

    #region Private Methods

    private Dictionary<uint, string> CreateFileNameHashLookup()
    {
      var lookup = new Dictionary<uint, string>();

      foreach ( var device in _devices )
      {
        foreach ( var node in device.EnumerateFiles() )
        {
          var fileName = node.Name;

          TryAddFileNameHashToLookup( ref lookup, fileName, "ps" );
          TryAddFileNameHashToLookup( ref lookup, fileName, "td" );
          TryAddFileNameHashToLookup( ref lookup, fileName, "sfx" );
        }
      }

      return lookup;
    }

    private static void TryAddFileNameHashToLookup( ref Dictionary<uint, string> lookup, string fileName, string ext )
    {
      fileName = Path.ChangeExtension( fileName, ext );
      var hash = Crc32.CalculateCrc32( fileName );

      if ( lookup.TryGetValue( hash, out var existingValue ) )
        ASSERT( existingValue == fileName );

      lookup[ hash ] = fileName;
    }

    private IFileSystemNode CreateCacheBlockEntryNodes(
      IFileSystemNode rootNode,
      CEACacheBlockFileNode cacheBlockFileNode,
      Dictionary<uint, string> fileNameLookup )
    {
      var cacheBlockNode = new CEAFileNode( this, cacheBlockFileNode.GetPath(), rootNode );

      var stream = cacheBlockFileNode.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );
      var cacheBlock = CacheBlock.Deserialize( reader );

      var entries = cacheBlock.Sections.SelectMany( x => x.Entries );
      foreach ( var entry in entries )
      {
        if ( !fileNameLookup.TryGetValue( ( uint ) entry.NameCrc, out var fileName ) )
          continue;

        cacheBlockNode.AddChild( CreateCacheBlockEntryNode( cacheBlockNode, entry, fileName ) );
      }

      return cacheBlockNode;
    }

    private IFileSystemNode CreateCacheBlockEntryNode( CEAFileNode parentNode, CacheBlockEntry entry, string fileName )
    {
      var extension = Path.GetExtension( fileName );
      switch ( extension )
      {
        case ".td":
          return new CEATextureDefinitionFileNode( this, fileName, entry.Data, parentNode );

        default:
          return new CEACacheBlockEntryFileNode( this, fileName, entry.Data, parentNode );
      }
    }

    #endregion

  }

}
