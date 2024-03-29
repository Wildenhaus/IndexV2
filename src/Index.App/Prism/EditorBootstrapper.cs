﻿using System;
using System.Windows;
using DryIoc;
using Index.App.Views;
using Index.Modules.DataExplorer;
using Index.Modules.JobManager;
using Index.Modules.Logging;
using Index.Modules.MeshEditor;
using Index.Modules.TextEditor;
using Index.Modules.TextureEditor;
using Index.UI.Commands;
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
    {
      var container = _dryIocContainer.Resolve<IContainerExtension>();

      GlobalCommands.Initialize( container );
      EditorCommands.Initialize( container );

      return container;
    }

    protected override DependencyObject CreateShell()
      => Container.Resolve<MainShell>();

    protected override void ConfigureModuleCatalog( IModuleCatalog moduleCatalog )
    {
      moduleCatalog.AddModule<LoggingModule>();
      moduleCatalog.AddModule<JobManagerModule>();
      moduleCatalog.AddModule<DataExplorerModule>();

      moduleCatalog.AddModule<TextEditorModule>();
      moduleCatalog.AddModule<TextureEditorModule>();
      moduleCatalog.AddModule<MeshEditorModule>();
    }

    protected override void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterDialogWindow<IxDialogWindow>();
      containerRegistry.RegisterDialog<UnhandledExceptionDialog>();
      containerRegistry.RegisterDialog<AboutView>();
    }

    protected override void OnInitialized()
    {
      AppDomain.CurrentDomain.UnhandledException += ( sender, e ) =>
      {
        App.Current.Dispatcher.Invoke( () =>
        {
          var exception = ( Exception ) e.ExceptionObject;
          var dialogService = Container.Resolve<IDialogService>();

          dialogService.ShowUnhandledExceptionDialog( exception );
        } );
      };

      if ( Shell is Window window )
      {
        Application.Current.MainWindow = window;
        window.Show();
      }
    }

  }

}
