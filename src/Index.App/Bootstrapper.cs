using System;
using System.Windows;
using Index.App.Views;
using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Domain.GameProfiles;
using Index.Domain.Models;
using Index.Modules.Database;
using Index.Modules.DataExplorer;
using Index.Modules.FileDialogs;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.App
{

  public class Bootstrapper : PrismBootstrapper
  {

    #region Overrides

    protected override DependencyObject CreateShell()
    {
      return new Window();
    }

    protected override void ConfigureModuleCatalog( IModuleCatalog moduleCatalog )
    {
      base.ConfigureModuleCatalog( moduleCatalog );

      moduleCatalog.AddModule<DatabaseModule>();
      moduleCatalog.AddModule<FileDialogsModule>();
      moduleCatalog.AddModule<DataExplorerModule>();
    }

    protected override void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterSingleton<IGameProfileManager, GameProfileManager>();
      containerRegistry.RegisterSingleton<IEditorEnvironment, EditorEnvironment>();
      containerRegistry.RegisterSingleton<IFileSystem, FileSystem>();
      containerRegistry.RegisterSingleton<IAssetManager, AssetManager>();
    }

    protected override void Initialize()
    {
      base.Initialize();
    }

    protected override void OnInitialized()
    {
      if ( !ShowLauncher() )
        Environment.Exit( 0 );

      InitializeEditor();

      var editor = Container.Resolve<EditorView>();
      editor.Closed += ( s, e ) => Environment.Exit( 0 );
      editor.Show();
    }

    #endregion

    #region Private Methods

    private bool ShowLauncher()
    {
      var launcherView = Container.Resolve<LauncherView>();
      var shouldLaunch = launcherView.ShowDialog() ?? false;
      if ( !shouldLaunch )
        return false;

      var profileManager = Container.Resolve<IGameProfileManager>();
      var editorEnvironment = Container.Resolve<IEditorEnvironment>();
      editorEnvironment.GameId = launcherView.Parameters.Get<string>( "GameId" );
      editorEnvironment.GameName = launcherView.Parameters.Get<string>( "GameName" );
      editorEnvironment.GamePath = launcherView.Parameters.Get<string>( "GamePath" );
      editorEnvironment.GameProfile = profileManager.Profiles[ editorEnvironment.GameId ];

      return true;
    }

    private bool InitializeEditor()
    {
      var loadingView = Container.Resolve<EditorLoadingView>();
      return loadingView.ShowDialog() ?? false;
    }

    #endregion

  }

}
