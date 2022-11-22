using System;
using System.Threading.Tasks;
using DryIoc;
using Index.Domain.Models;
using Prism.Ioc;
using Prism.Mvvm;

namespace Index.App.ViewModels
{

  public class EditorLoadingViewModel : BindableBase
  {

    #region Events

    public event EventHandler? Complete;
    public event EventHandler? Faulted;

    #endregion

    #region Data Members

    private IContainer _container;
    private IEditorEnvironment _environment;

    private string _status;

    #endregion

    #region Properties

    public string Status
    {
      get => _status;
      set => SetProperty( ref _status, value );
    }

    public string VersionName
    {
      get
      {
        var assembly = GetType().Assembly;
        var version = assembly.GetName().Version.ToString();
        return $"v{version}";
      }
    }

    #endregion

    #region Constructor

    public EditorLoadingViewModel( IContainer container, IEditorEnvironment editorEnvironment )
    {
      _container = container;
      _environment = editorEnvironment;

      _status = "Initializing...";
    }

    #endregion

    #region Private Methods

    internal async Task Initialize()
    {
      try
      {
        await RunTask( "Initializing FileSystem", InitializeFileSystem );
        await RunTask( "Initializing AssetManager", InitializeAssetManager );

        Complete?.Invoke( this, EventArgs.Empty );

      }
      catch ( Exception ex )
      {
        Faulted?.Invoke( this, EventArgs.Empty );
      }
    }

    private async Task InitializeFileSystem()
    {
      var loader = _environment.GameProfile.FileSystemLoader;
      var fileSystem = _environment.FileSystem;

      loader.SetBasePath( _environment.GamePath );
      await fileSystem.LoadDevices( loader );
    }

    private void InitializeAssetManager()
    {
      var assetManager = _environment.AssetManager;
      var fileSystem = _environment.FileSystem;

      assetManager.InitializeFromFileSystem( fileSystem );
    }

    private async Task RunTask( string status, Func<Task> taskFactory )
    {
      SetStatus( status );
      await taskFactory();
    }

    private async Task RunTask( string status, Action action )
    {
      SetStatus( status );
      await Task.Factory.StartNew( action, TaskCreationOptions.LongRunning );
    }

    private void SetStatus( string status )
      => Status = $"{status}...";

    #endregion

  }

}
