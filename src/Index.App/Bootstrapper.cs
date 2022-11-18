using System;
using System.Windows;
using Index.App.Models;
using Index.App.Views;
using Index.Domain.FileSystem;
using Index.Domain.GameProfiles;
using Index.Domain.Models;
using Index.Modules.Database;
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
    }

    protected override void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterSingleton<IGameProfileManager, GameProfileManager>();
      containerRegistry.RegisterSingleton<IEditorEnvironment, EditorEnvironment>();
      containerRegistry.RegisterSingleton<IFileSystem, FileSystem>();
    }

    protected override void OnInitialized()
    {
      if ( !ShowLauncher() )
        Environment.Exit( 0 );

      InitializeEditor();

      //if ( Shell is Window window )
      //  window.Show();
    }

    #endregion

    #region Private Methods

    private bool ShowLauncher()
    {
      var launcherView = Container.Resolve<LauncherView>();
      var shouldLaunch = launcherView.ShowDialog() ?? false;
      if ( !shouldLaunch )
        return false;

      var editorEnvironment = Container.Resolve<IEditorEnvironment>();
      editorEnvironment.GameId = launcherView.Parameters.Get<string>( "GameId" );
      editorEnvironment.GameName = launcherView.Parameters.Get<string>( "GameName" );
      editorEnvironment.GamePath = launcherView.Parameters.Get<string>( "GamePath" );

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
