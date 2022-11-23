﻿using System;
using System.Windows;
using DryIoc;
using Index.App.Views;
using Index.Modules.DataExplorer;
using Index.Modules.Logging;
using Index.UI.Windows;
using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Services.Dialogs;

namespace Index.App.Prism
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
      containerRegistry.RegisterDialogWindow<IxDialogWindow>();
      containerRegistry.RegisterDialog<UnhandledExceptionDialog>();
    }

    protected override void OnInitialized()
    {
      AppDomain.CurrentDomain.UnhandledException += ( sender, e ) =>
      {
        var exception = ( Exception ) e.ExceptionObject;
        var dialogService = Container.Resolve<IDialogService>();

        dialogService.ShowUnhandledExceptionDialog( exception );
      };

      if ( Shell is Window window )
      {
        Application.Current.MainWindow = window;
        window.Show();
      }
    }

  }

}
