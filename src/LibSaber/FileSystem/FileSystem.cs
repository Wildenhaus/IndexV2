using LibSaber.Common;

namespace LibSaber.FileSystem
{

  public class FileSystem : DisposableObject, IFileSystem
  {

    #region Data Members

    private readonly Dictionary<string, IFileSystemDevice> _devices;

    #endregion

    #region Properties

    public IReadOnlyDictionary<string, IFileSystemDevice> Devices
    {
      get => _devices;
    }

    #endregion

    #region Constructor

    public FileSystem()
    {
      _devices = new Dictionary<string, IFileSystemDevice>();
    }

    #endregion

    #region Public Methods

    public void AttachDevice( IFileSystemDevice device )
    {
      if ( !device.IsInitialized )
        device.Initialize();

      var deviceName = device.Root.Name;
      lock ( _devices )
        _devices.Add( deviceName, device );
    }

    public void DetachDevice( IFileSystemDevice device )
    {
      lock ( _devices )
        _devices.Remove( device.Root.Name );
    }

    public IEnumerable<IFileSystemDevice> EnumerateDevices()
      => _devices.Values;

    public IEnumerable<IFileSystemNode> EnumerateDirectories()
      => EnumerateDevices().SelectMany( x => x.EnumerateDirectories() );

    public IEnumerable<IFileSystemNode> EnumerateFiles()
      => EnumerateDevices().SelectMany( x => x.EnumerateFiles() );

    public IEnumerable<IFileSystemNode> EnumerateNodes()
      => EnumerateDevices().SelectMany( x => x.EnumerateNodes() );

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      foreach ( var device in _devices.Values )
        device?.Dispose();

      _devices.Clear();
    }

    #endregion

  }

}
