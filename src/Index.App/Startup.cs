using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using DryIoc;
using Index.App.Prism;
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
using Microsoft.EntityFrameworkCore;
using Prism.DryIoc;
using Prism.Ioc;

namespace Index.App
{

  internal class Startup
  {

    #region Data Members

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

      InitializeDatabase();

      if ( !TryApplyDebugLaunchArgs() )
        if ( !ShowLauncher() )
          Environment.Exit( 0 );

      if ( !InitializeEditor() )
        Environment.Exit( 0 );
    }

    private void InitializeDatabase()
    {
      var context = Container.Resolve<IndexDataContext>();
      context.Database.Migrate();
    }

    private bool ShowLauncher()
    {
      var launcherView = Container.Resolve<LauncherView>();
      return launcherView.ShowDialog() ?? false;
    }

    private bool InitializeEditor()
    {
      var loadingView = Container.Resolve<EditorLoadingView>();
      var result = loadingView.ShowDialog();

      return true;
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

    #endregion

  }

}
