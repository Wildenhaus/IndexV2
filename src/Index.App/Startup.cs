using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using DryIoc;
using Index.App.Prism;
using Index.App.ViewModels;
using Index.App.Views;
using Index.Domain.Assets;
using Index.Domain.Database;
using Index.Domain.Database.Repositories;
using Index.Domain.FileSystem;
using Index.Domain.GameProfiles;
using Index.Domain.Models;
using Index.Jobs;
using Index.Textures;
using Index.UI.Services;
using Index.UI.Windows;
using Microsoft.EntityFrameworkCore;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace Index.App
{

  internal class Startup
  {

    #region Data Members

    private SplashView _splash;

    protected Container Container { get; }

    #endregion

    #region Constructor

    public Startup()
    {
      Container = new Container( CreateContainerRules() );
      ( ( App ) App.Current ).Container = Container;

      var containerExtension = new DryIocContainerExtension( Container );
      Container.RegisterInstance<IContainerExtension>( containerExtension );
      Container.RegisterInstance<IContainerProvider>( containerExtension );
    }

    #endregion

    public void Run()
    {
      ShowSplash();

      RegisterServices();
      RunStartupFlow();
      RunEditor();
    }

    #region Service Registration

    private Rules CreateContainerRules()
    {
      return Rules.Default
        .WithConcreteTypeDynamicRegistrations( reuse: Reuse.Transient )
        .With( Made.Of( FactoryMethod.ConstructorWithResolvableArguments ) )
        .WithFuncAndLazyWithoutRegistration()
        .WithTrackingDisposableTransients()
        .WithoutFastExpressionCompiler()
        .WithFactorySelector( Rules.SelectLastRegisteredFactory() );
    }

    private void RegisterServices()
    {
      RegisterLogger();
      RegisterDatabaseServices();
      RegisterEditorServices();
    }

    private void RegisterLogger()
    {
      // Default Logger
      Container.Register( Made.Of( () => Serilog.Log.Logger ),
          setup: Setup.With( condition: r => r.Parent.ImplementationType == null ) );

      // Context-Specific Logger
      Container.Register(
          Made.Of( () => Serilog.Log.ForContext( Arg.Index<Type>( 0 ) ), r => r.Parent.ImplementationType ),
          setup: Setup.With( condition: r => r.Parent.ImplementationType != null ) );
    }

    private void RegisterDatabaseServices()
    {
      Container.Register<IndexDataContext>( Reuse.Transient );
      Container.Register<IGamePathRepository, GamePathRepository>( Reuse.Transient );
      Container.Register<ISavedSettingsRepository, SavedSettingsRepository>( Reuse.Transient );
    }

    private void RegisterEditorServices()
    {
      Container.Register<IDxgiTextureService, DxgiTextureService>();
      Container.Register<IFileDialogService, FileDialogService>( Reuse.Singleton );

      Container.Register<IFileSystem, FileSystem>( Reuse.Singleton );
      Container.Register<IAssetManager, AssetManager>( Reuse.Singleton );
      Container.Register<IGameProfileManager, GameProfileManager>( Reuse.Singleton );
      Container.Register<IJobManager, JobManager>( Reuse.Singleton );
      Container.Register<IEditorEnvironment, EditorEnvironment>( Reuse.Singleton );
    }

    #endregion

    #region Startup Flow

    private void RunStartupFlow()
    {
      App.Current.MainWindow = new Window();
      App.Current.DispatcherUnhandledException += OnUnhandledException;

      InitializeDatabase();

      if ( !TryApplyDebugLaunchArgs() )
        if ( !ShowLauncher() )
          Environment.Exit( 0 );

      if ( !InitializeEditor() )
        Environment.Exit( 0 );

      App.Current.DispatcherUnhandledException -= OnUnhandledException;
    }

    private void InitializeDatabase()
    {
      var context = Container.Resolve<IndexDataContext>();
      context.Database.Migrate();
    }

    private bool ShowLauncher()
    {
      var launcherView = Container.Resolve<LauncherView>();

      HideSplash();
      return launcherView.ShowDialog() ?? false;
    }

    private bool InitializeEditor()
    {
      var loadingView = Container.Resolve<EditorLoadingView>();

      HideSplash();
      var result = loadingView.ShowDialog();

      if ( result == false && loadingView.Exception != null )
        ShowUnhandledExceptionDialog( loadingView.Exception );

      return result ?? true;
    }

    private void RunEditor()
    {
      var bootstrapper = new EditorBootstrapper( Container );
      bootstrapper.Run();
    }

    private bool TryApplyDebugLaunchArgs()
    {
      if ( !Debugger.IsAttached )
        return false;

      var launchArgs = Environment.GetCommandLineArgs();
      if ( launchArgs.Length < 3 )
        return false;

      var gameId = launchArgs[ 1 ];
      var gamePath = launchArgs[ 2 ];
      if ( !Directory.Exists( gamePath ) )
        return false;

      var profileManager = Container.Resolve<IGameProfileManager>();
      if ( !profileManager.Profiles.TryGetValue( gameId, out var profile ) )
        return false;

      var editorEnvironment = Container.Resolve<IEditorEnvironment>();
      editorEnvironment.GameId = profile.GameId;
      editorEnvironment.GameName = profile.GameName;
      editorEnvironment.GamePath = gamePath;
      editorEnvironment.GameProfile = profile;

      return true;
    }

    private void ShowUnhandledExceptionDialog( Exception exception )
    {
      var parameters = new DialogParameters { { nameof( Exception ), exception } };

      var exceptionViewModel = new UnhandledExceptionDialogViewModel( null );
      exceptionViewModel.OnDialogOpened( parameters );

      var exceptionView = new UnhandledExceptionDialog();

      var exceptionWindow = new IxDialogWindow();
      exceptionWindow.DataContext = exceptionViewModel;
      exceptionWindow.Content = exceptionView;

      exceptionWindow.ShowDialog();
    }

    private void ShowSplash()
    {
      if ( _splash is not null )
        return;

      _splash = new SplashView();
      _splash.Show();
    }

    private void HideSplash()
    {
      if ( _splash is null )
        return;

      _splash.Close();
      _splash = null;
    }

    #endregion

    #region Event Handlers

    private void OnUnhandledException( object sender, DispatcherUnhandledExceptionEventArgs e )
    {
      // A temporary event hook for showing exception windows before the editor is loaded.
      ShowUnhandledExceptionDialog( e.Exception );
    }

    #endregion

  }

}
