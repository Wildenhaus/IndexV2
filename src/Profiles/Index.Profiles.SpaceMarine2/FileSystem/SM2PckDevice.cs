using System.Threading.Tasks.Dataflow;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.FileSystem.Files;
using LibSaber.IO;
using LibSaber.SpaceMarine2.Serialization;
using LibSaber.SpaceMarine2.Structures.Resources;
using ResLoader = System.Threading.Tasks.Dataflow.ActionBlock<System.Func<System.Threading.Tasks.Task>>;

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

    var resLoader = new ResLoader( async factory => await factory(),
      new() { MaxDegreeOfParallelism = Environment.ProcessorCount } );

    foreach ( var entry in _zipFile.Entries.Values )
    {
      CreateNode( entry, rootNode, resLoader );
    }

    resLoader.Complete();
    resLoader.Completion.Wait();

    return rootNode;
  }

  private void CreateNode( 
    fioZIP_CACHE_FILE.ENTRY entry, 
    IFileSystemNode parent,
    ResLoader resLoader)
  {
    SM2FileSystemNode node = null;

    var ext = Path.GetExtension( entry.FileName );

    if ( ext == ".resource" )
      node = CreateResourceFileNode( entry, parent, resLoader );
    else
      node = new SM2FileSystemNode( this, entry, parent );

    if ( node != null )
      parent.AddChild( node );
  }

  private SM2FileSystemNode CreateResourceFileNode( 
    fioZIP_CACHE_FILE.ENTRY entry,
    IFileSystemNode parent,
    ResLoader resLoader )
  {
    var fileName = entry.FileName.Replace( ".resource", "" );
    var resourceExt = Path.GetExtension( fileName );

    switch(resourceExt)
    {
      case ".pct":
        return BeginInitResourceNode( new SM2TextureResourceFileNode( this, entry, parent ), resLoader );
      case ".tpl":
        return BeginInitResourceNode( new SM2TemplateResourceFileNode( this, entry, parent ), resLoader );
      case ".td":
        return BeginInitResourceNode( new SM2TextureDefinitionResourceFileNode(this, entry, parent ), resLoader );
      case ".scn":
        return BeginInitResourceNode( new SM2SceneResourceFileNode( this, entry, parent ), resLoader );
      default:
        return new SM2FileSystemNode( this, entry, parent );
    }
  }

  private SM2FileSystemNode BeginInitResourceNode<TResDesc>(SM2ResourceFileNode<TResDesc> node, ResLoader resLoader )
    where TResDesc : resDESC
  {
    resLoader.Post(() => Task.Run( () =>
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
    } ) );

    return node;
  }

  #endregion

}
