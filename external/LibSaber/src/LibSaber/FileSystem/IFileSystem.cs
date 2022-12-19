namespace LibSaber.FileSystem
{

  public interface IFileSystem : IDisposable
  {

    #region Properties

    IReadOnlyDictionary<string, IFileSystemDevice> Devices { get; }

    #endregion

    #region Public Methods

    void AttachDevice( IFileSystemDevice device );
    void DetachDevice( IFileSystemDevice device );

    IEnumerable<IFileSystemDevice> EnumerateDevices();
    IEnumerable<IFileSystemNode> EnumerateNodes();
    IEnumerable<IFileSystemNode> EnumerateDirectories();
    IEnumerable<IFileSystemNode> EnumerateFiles();

    #endregion

  }

}
