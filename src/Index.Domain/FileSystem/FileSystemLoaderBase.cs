namespace Index.Domain.FileSystem
{

  public abstract class FileSystemLoaderBase : IFileSystemLoader
  {

    #region Data Members

    private string _basePath;
    private readonly HashSet<string> _deviceFileNames;
    private readonly List<IFileSystemDevice> _loadedDevices;

    #endregion

    #region Properties

    protected IReadOnlyList<IFileSystemDevice> Devices => _loadedDevices;

    #endregion

    #region Constructor

    protected FileSystemLoaderBase()
    {
      _deviceFileNames = new HashSet<string>();
      _loadedDevices = new List<IFileSystemDevice>();
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
      return _loadedDevices;
    }

    #endregion

    #region Private Methods

    protected abstract Task OnLoadDevices();

    protected async Task AddDevice( IFileSystemDevice device )
    {
      var result = await device.InitializeAsync();

      if ( result.IsSuccessful )
        _loadedDevices.Add( device );
    }

    protected async Task LoadDevice( string path, Func<string, IFileSystemDevice> deviceFactory )
    {
      var device = deviceFactory( path );
      var result = await device.InitializeAsync();

      if ( result.IsSuccessful )
        _loadedDevices.Add( device );
    }

    protected async Task LoadFilesWithExtension( string extension, Func<string, IFileSystemDevice> deviceFactory, IEnumerable<string> excludedFileNames = null )
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

        await LoadDevice( filePath, deviceFactory );
      }
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
