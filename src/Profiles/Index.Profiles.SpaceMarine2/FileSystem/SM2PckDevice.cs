using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.FileSystem.Files;
using LibSaber.IO;
using LibSaber.SpaceMarine2.Serialization;
using LibSaber.SpaceMarine2.Structures.Resources;

namespace Index.Profiles.SpaceMarine2.FileSystem;

public class SM2PckDevice : FileSystemDeviceBase
{

  #region Data Members

  private readonly string _basePath;
  private readonly string _filePath;
  private readonly fioZIP_FILE _zipFile;

  #endregion

  #region Constructor

  public SM2PckDevice( string basePath, string filePath )
  {
    _basePath = basePath;
    _filePath = filePath;
    _zipFile = fioZIP_FILE.Open( _filePath );
  }

  #endregion

  #region Overrides

  public override Stream GetStream( IFileSystemNode node )
  {
    var smNode = node as SM2FileSystemNode;
    ASSERT( smNode != null, "Node is not an SM2FileSystemNode." );

    return _zipFile.GetFileStream( smNode.Entry );
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
    _zipFile?.Dispose();
    base.OnDisposing();
  }

  #endregion

  #region Private Methods

  private IFileSystemNode InitNodes()
  {
    var fileName = _filePath.Replace( _basePath, "" );
    var rootNode = new SM2FileSystemNode( this, fileName );

    var taskList = new List<Task>();
    foreach ( var entry in _zipFile.Entries.Values )
    {
      CreateNode( entry, rootNode, taskList );
    }

    Task.WaitAll( taskList.ToArray() );

    return rootNode;
  }

  private void CreateNode( 
    fioZIP_CACHE_FILE.ENTRY entry, 
    IFileSystemNode parent,
    List<Task> taskList)
  {
    SM2FileSystemNode node = null;

    var ext = Path.GetExtension( entry.FileName );

    if ( ext == ".resource" )
      node = CreateResourceFileNode( entry, parent, taskList );
    else
      node = new SM2FileSystemNode( this, entry, parent );

    if ( node != null )
      parent.AddChild( node );
  }

  private SM2FileSystemNode CreateResourceFileNode( 
    fioZIP_CACHE_FILE.ENTRY entry,
    IFileSystemNode parent,
    List<Task> taskList)
  {
    var fileName = entry.FileName.Replace( ".resource", "" );
    var resourceExt = Path.GetExtension( fileName );

    switch(resourceExt)
    {
      case ".pct":
        return BeginInitResourceNode( new SM2TextureResourceFileNode( this, entry, parent ), taskList );
      case ".tpl":
        return BeginInitResourceNode( new SM2TemplateResourceFileNode( this, entry, parent ), taskList );
      case ".td":
        return BeginInitResourceNode( new SM2TextureDefinitionResourceFileNode(this, entry, parent ), taskList );
      //case ".scn":
      //  return BeginInitResourceNode( new SM2SceneResourceFileNode( this, entry, parent ), taskList );
      default:
        return new SM2FileSystemNode( this, entry, parent );
    }
  }

  private SM2FileSystemNode BeginInitResourceNode<TResDesc>(SM2ResourceFileNode<TResDesc> node, List<Task> taskList)
    where TResDesc : resDESC
  {
    var task = Task.Run( () =>
    {
      var entry = node.Entry;
      using var descStream = _zipFile.GetFileStream( node.Entry );
      var reader = new NativeReader( descStream, Endianness.LittleEndian );

      var desc = Serializer<resDESC>.Deserialize( reader );

      var expectedType = typeof(TResDesc);
      var actualType = desc.GetType();
      if( actualType != expectedType )
      {
        FAIL(
          "resDESC deserialized to an unexpected type!\n" +
          $"File: {entry.FileName}\n" +
          $"Expected type: {expectedType.Name}\n" +
          $"Actual type: {actualType.Name}" );
      }

      node.ResourceDescription = desc as TResDesc;
    } );

    taskList.Add( task );
    return node;
  }

  #endregion

}
