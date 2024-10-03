using System.Threading.Tasks.Dataflow;

namespace Index.Domain.FileSystem
{

  public abstract class FileSystemLoaderBase : IFileSystemLoader
  {

    #region Constants

    const int THREAD_COUNT = 4;

    #endregion

    #region Events

    public event Action<double> ProgressChanged;

    #endregion

    #region Data Members

    private string _basePath;
    private readonly HashSet<string> _deviceFileNames;
    private readonly List<IFileSystemDevice> _loadedDevices;

    private Queue<IFileSystemDevice> _devicesToLoad;

    #endregion

    #region Properties

    protected string BasePath => _basePath;
    protected IReadOnlyList<IFileSystemDevice> Devices => _loadedDevices;

    #endregion

    #region Constructor

    protected FileSystemLoaderBase()
    {
      _deviceFileNames = new HashSet<string>();
      _loadedDevices = new List<IFileSystemDevice>();

      _devicesToLoad = new Queue<IFileSystemDevice>();
    }

    #endregion

    #region Public Methods

    public void SetBasePath( string basePath )
    {
      ASSERT( !string.IsNullOrWhiteSpace( basePath ), "Invalid path." );
      ASSERT( Directory.Exists( basePath ), "Directory does not exist: {0}", basePath );

      _basePath = basePath;
    }

    public async Task<IReadOnlyList<IFileSystemDevice>> LoadDevices()
    {
      ASSERT( !string.IsNullOrWhiteSpace( _basePath ), "SetBasePath must be called before loading devices." );

      await OnLoadDevices();
      await InitializeDevices();

      return _loadedDevices;
    }

    #endregion

    #region Private Methods

    protected abstract Task OnLoadDevices();

    protected async Task AddDevice( IFileSystemDevice device )
    {
      var result = await device.InitializeAsync();

      if ( result.IsSuccessful )
      {
        lock(_loadedDevices)
          _loadedDevices.Add( device );
      }
    }

    protected Task LoadDevice( string path, Func<string, IFileSystemDevice> deviceFactory )
    {
      var device = deviceFactory( path );
      AddDeviceToLoadQueue( device );

      return Task.CompletedTask;
    }

    protected Task LoadFileWithName( string name, Func<string, IFileSystemDevice> deviceFactory )
    {
      foreach ( var filePath in Directory.EnumerateFiles( _basePath, name, SearchOption.AllDirectories ) )
      {
        var fileName = Path.GetFileNameWithoutExtension( filePath ).ToLower();
        LoadDevice( filePath, deviceFactory );
        break;
      }

      return Task.CompletedTask;
    }

    protected Task LoadFilesWithExtension( string extension, Func<string, IFileSystemDevice> deviceFactory, IEnumerable<string> excludedFileNames = null )
    {
      if ( excludedFileNames is null )
        excludedFileNames = Enumerable.Empty<string>();

      var excludeSet = new HashSet<string>( excludedFileNames.Select( x => x.ToLower() ) );

      extension = SanitizeExtension( extension );

      foreach ( var filePath in Directory.EnumerateFiles( _basePath, extension, SearchOption.AllDirectories ) )
      {
        var fileName = Path.GetFileNameWithoutExtension( filePath ).ToLower();
        if ( excludeSet.Contains( fileName ) )
          continue;

        // Skip files that have already been loaded
        if ( !_deviceFileNames.Add( filePath ) )
          continue;

        LoadDevice( filePath, deviceFactory );
      }

      return Task.CompletedTask;
    }

    private void AddDeviceToLoadQueue( IFileSystemDevice device )
    {
      lock ( _devicesToLoad )
        _devicesToLoad.Enqueue( device );
    }

    private async Task InitializeDevices()
    {
      int loadedDevicesCount = 0;
      int totalDevicesCount = _devicesToLoad.Count;

#if DEBUG
      int threadCount = Environment.ProcessorCount;
#else
      int threadCount = 4;
#endif

      void IncrementLoadProgress()
      {
        var completed = Interlocked.Increment( ref loadedDevicesCount );
        var progress = ( double ) completed / totalDevicesCount;
        ProgressChanged?.Invoke( progress );
      }

      var initBlock = new ActionBlock<IFileSystemDevice>( async device =>
      {
        var result = await device.InitializeAsync();
        if ( result.IsSuccessful )
        {
          lock ( _loadedDevices )
            _loadedDevices.Add( device );
        }

        IncrementLoadProgress();
      }, new() { EnsureOrdered = false, MaxDegreeOfParallelism = threadCount } );


      while ( _devicesToLoad.TryDequeue( out var device ) )
        initBlock.Post( device );

      initBlock.Complete();
      await initBlock.Completion;
    }

    private static string SanitizeExtension( string extension )
    {
      if ( extension.StartsWith( "*." ) )
        return extension;

      if ( extension.StartsWith( "." ) )
        return $"*{extension}";

      return $"*.{extension}";
    }

#endregion

  }

}
