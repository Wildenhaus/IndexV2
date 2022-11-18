namespace Index.Domain.FileSystem
{

  public abstract class FileSystemDeviceBase : DisposableObject, IFileSystemDevice
  {

    #region Properties

    public bool IsInitialized { get; private set; }
    public IFileSystemNode Root { get; private set; }
    public IParameterCollection Metadata { get; }

    #endregion

    #region Constructor

    protected FileSystemDeviceBase()
    {
      Metadata = new ParameterCollection();
    }

    #endregion

    #region Public Methods

    public IResult Initialize()
    {
      var initTask = InitializeAsync().ConfigureAwait( false );
      return initTask.GetAwaiter().GetResult();
    }

    public async Task<IResult> InitializeAsync( CancellationToken cancellationToken = default )
    {
      if ( IsInitialized )
        return Result.Successful( "Device has already been initialized." );

      try
      {
        var initResult = await OnInitializing( cancellationToken );

        if ( initResult.IsSuccessful )
        {
          IsInitialized = true;
          Root = initResult.Value;
        }

        return initResult;
      }
      catch ( Exception ex )
      {
        return Result.Unsuccessful( ex, "Device failed to initialize." );
      }
    }

    public abstract IResult<Stream> GetStream( IFileSystemNode node );

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

    protected abstract Task<IResult<IFileSystemNode>> OnInitializing( CancellationToken cancellationToken = default );

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      Metadata?.Dispose();

      Root?.Dispose();
      Root = null;
    }

    #endregion

  }

}
