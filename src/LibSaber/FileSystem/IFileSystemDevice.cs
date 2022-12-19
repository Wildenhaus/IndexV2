namespace LibSaber.FileSystem
{

  public interface IFileSystemDevice : IDisposable
  {

    #region Properties

    bool IsInitialized { get; }
    bool IsDisposed { get; }

    IFileSystemNode Root { get; }

    #endregion

    #region Public Methods

    bool Initialize();
    Stream GetStream( IFileSystemNode node );

    IEnumerable<IFileSystemNode> EnumerateNodes();
    IEnumerable<IFileSystemNode> EnumerateDirectories();
    IEnumerable<IFileSystemNode> EnumerateFiles();

    #endregion

  }

}
