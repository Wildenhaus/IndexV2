using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using DryIoc;
using Index.App.Views;
using Index.Common;
using Index.Domain.Models;
using Prism.Mvvm;

namespace Index.App.ViewModels
{

  public class EditorLoadingViewModel : BindableBase
  {

    #region Events

    public event EventHandler? Complete;
    public event EventHandler<Exception>? Faulted;

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
        return AssemblyHelpers.GetBuildString( assembly );
      }
    }

    public Color IconColor
    {
      get => Color.FromRgb( 22, 200, 186 );
    }

    public Brush IconBrush
    {
      get => new SolidColorBrush( IconColor );
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
        await RunTask( "Initializing Profile", InitializeProfile );

        Complete?.Invoke( this, EventArgs.Empty );

      }
      catch ( Exception ex )
      {
        Faulted?.Invoke( this, ex );
      }
    }

    private async Task InitializeFileSystem()
    {
      var fileSystem = _environment.FileSystem;
      var loader = _environment.GameProfile.FileSystemLoader;
      loader.SetBasePath( _environment.GamePath );

      void SetLoadProgress( double progress )
      {
        Application.Current.Dispatcher.Invoke( () =>
        {
          ( Application.Current.Windows[ ^1 ] as EditorLoadingView )?.AnimateProgress( progress );
        } );
      }

      loader.ProgressChanged += SetLoadProgress;
      await fileSystem.LoadDevices( loader );
      loader.ProgressChanged -= SetLoadProgress;
    }

    private void InitializeAssetManager()
    {
      var assetManager = _environment.AssetManager;
      var fileSystem = _environment.FileSystem;

      assetManager.InitializeFromFileSystem( fileSystem );
    }

    private async Task InitializeProfile()
    {
      var profile = _environment.GameProfile;
      await profile.Initialize( _environment );
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
