using LibSaber.Common;

namespace LibSaber.FileSystem
{

  public abstract class FileSystemDevice : DisposableObject, IFileSystemDevice
  {

    #region Properties

    public bool IsInitialized { get; private set; }
    public IFileSystemNode Root { get; private set; }

    #endregion

    #region Public Methods


    public bool Initialize()
    {
      if ( IsInitialized )
        return true;

      Root = OnInitializing();

      if ( Root is null )
        return false;

      IsInitialized = true;
      return true;
    }

    public abstract Stream GetStream( IFileSystemNode node );

    public IEnumerable<IFileSystemNode> EnumerateNodes()
    {
      yield return Root;
      foreach ( var child in Root.EnumerateChildren( recursive: true ) )
        yield return child;
    }

    public IEnumerable<IFileSystemNode> EnumerateDirectories()
      => EnumerateNodes().Where( node => node.IsDirectory );

    public IEnumerable<IFileSystemNode> EnumerateFiles()
      => EnumerateNodes().Where( node => !node.IsDirectory );

    #endregion

    #region Private Methods

    protected abstract IFileSystemNode OnInitializing();

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      Root?.Dispose();
      Root = null;
    }

    #endregion

  }

}
