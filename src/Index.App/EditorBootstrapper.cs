using System;
using System.Windows;
using DryIoc;
using Index.App.Views;
using Index.Modules.DataExplorer;
using Index.Modules.Logging;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.App
{

  internal class EditorBootstrapper : PrismBootstrapper
  {

    private readonly Container _dryIocContainer;

    public EditorBootstrapper( Container dryIocContainer )
    {
      ASSERT_NOT_NULL( dryIocContainer );
      _dryIocContainer = dryIocContainer;
    }

    protected override IContainerExtension CreateContainerExtension()
      => new DryIocContainerExtension( _dryIocContainer );

    protected override DependencyObject CreateShell()
      => Container.Resolve<EditorView>();

    protected override void ConfigureModuleCatalog( IModuleCatalog moduleCatalog )
    {
      moduleCatalog.AddModule<LoggingModule>();
      moduleCatalog.AddModule<DataExplorerModule>();
    }

    protected override void RegisterTypes( IContainerRegistry containerRegistry )
    {
    }

    protected override void OnInitialized()
    {
      if ( Shell is Window window )
      {
        App.Current.MainWindow = window;
        window.Show();
      }
    }

  }

}
