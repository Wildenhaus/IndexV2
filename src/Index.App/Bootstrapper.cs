using System;
using System.Windows;
using Index.App.ViewModels;
using Index.App.Views;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Services.Dialogs;

namespace Index.App
{

  public class Bootstrapper : PrismBootstrapper
  {

    protected override DependencyObject CreateShell()
    {
      return new Window();
    }

    protected override void RegisterTypes( IContainerRegistry containerRegistry )
    {
    }

    protected override void OnInitialized()
    {
      var launcherView = Container.Resolve<LauncherView>();
      var shouldLaunch = launcherView.ShowDialog() ?? false;
      if ( !shouldLaunch )
        Environment.Exit( 0 );

      //if ( Shell is Window window )
      //  window.Show();
    }

  }

}
