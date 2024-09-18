using System.IO.Compression;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.FileSystem.Files;

namespace Index.Profiles.SpaceMarine2.FileSystem;

public class SM2PckDevice : FileSystemDeviceBase
{

  #region Data Members

  private readonly string _basePath;
  private readonly string _filePath;
  private readonly ZipArchive _zipArchive;

  #endregion

  #region Constructor

  public SM2PckDevice( string basePath, string filePath )
  {
    _basePath = basePath;
    _filePath = filePath;
    _zipArchive = ZipFile.OpenRead( _filePath );
  }

  #endregion

  #region Overrides

  public override Stream GetStream( IFileSystemNode node )
  {
    lock ( _zipArchive )
    {
      var smNode = node as SM2FileSystemNode;
      ASSERT( smNode != null, "Node is not an SM2FileSystemNode." );

      var stream = new MemoryStream();
      using ( var smStream = smNode.Entry.Open() )
        smStream.CopyTo( stream );

      stream.Position = 0;
      return stream;
    }
  }

  protected override Task<IResult<IFileSystemNode>> OnInitializing( CancellationToken cancellationToken = default )
  {
    return Task.Run( () =>
    {
      var rootNode = InitNodes();
      return ( IResult<IFileSystemNode> ) Result.Successful( rootNode );
    } );
  }

  protected override void OnDisposing()
  {
    _zipArchive?.Dispose();
    base.OnDisposing();
  }

  #endregion

  #region Private Methods

  private IFileSystemNode InitNodes()
  {
    var fileName = _filePath.Replace(_basePath, "");
    var rootNode = new SM2FileSystemNode( this, fileName );

    foreach ( var entry in _zipArchive.Entries )
    {
      CreateNode( entry, rootNode );
    }

    return rootNode;
  }

  private void CreateNode( ZipArchiveEntry entry, IFileSystemNode parent )
  {
    SM2FileSystemNode node = null;

    if ( entry.Name.EndsWith( ".pct.resource" ) )
      node = new SM2TextureFileNode( this, entry, parent );
    else
    {
      switch ( Path.GetExtension( entry.Name ) )
      {
        case ".tpl":
          node = new SM2TemplateFileNode( this, entry, parent );
          break;
        default:
          node = new SM2FileSystemNode( this, entry, parent );
          break;
      }
    }

    if(node != null)
      parent.AddChild( node );
  }

  #endregion

}
