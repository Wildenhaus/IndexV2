namespace Index.Domain.FileSystem
{

  public interface IFileSystem : IDisposable
  {

    IReadOnlyDictionary<string, IFileSystemDevice> Devices { get; }

    void AttachDevice( IFileSystemDevice device );
    void DetachDevice( IFileSystemDevice device );

    Task LoadDevices( IFileSystemLoader deviceLoader );

    IEnumerable<IFileSystemDevice> EnumerateDevices();
    IEnumerable<IFileSystemNode> EnumerateNodes();
    IEnumerable<IFileSystemNode> EnumerateDirectories();
    IEnumerable<IFileSystemNode> EnumerateFiles();

  }

}
