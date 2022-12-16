﻿using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using Index.Domain;
using Index.Domain.Assets;
using Index.Jobs;
using Index.UI.ViewModels;
using Index.Utilities;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;

namespace Index.UI.Commands
{

  public static class EditorCommands
  {

    #region Data Members

    private static IContainerProvider _container;

    #endregion

    #region Commands

    public static ICommand OpenTabCommand { get; private set; }
    public static ICommand CloseTabCommand { get; private set; }
    public static ICommand CloseAllTabsCommand { get; private set; }
    public static ICommand CloseAllTabsButThisCommand { get; private set; }

    public static ICommand ExportAssetCommand { get; private set; }
    public static ICommand ExportAssetReferenceCommand { get; private set; }

    #endregion

    #region Public Methods

    public static void Initialize( IContainerProvider container )
    {
      _container = container;

      OpenTabCommand = new DelegateCommand<IAssetReference>( OpenTab );
      CloseTabCommand = new DelegateCommand<IAssetReference>( CloseTab );
      CloseAllTabsCommand = new DelegateCommand( CloseAllTabs );
      CloseAllTabsButThisCommand = new DelegateCommand<IAssetReference>( CloseAllTabsButThis );

      ExportAssetCommand = new DelegateCommand<IExportableAsset>( ExportAsset );
    }

    #endregion

    #region Command Methods

    private static void OpenTab( IAssetReference assetReference )
    {
      var parameters = new NavigationParameters
      {
        { "AssetReference", assetReference },
        { "AssetName", assetReference.AssetName },
        { "AssetType", assetReference.AssetType }
      };

      var regionManager = _container.Resolve<IRegionManager>();
      regionManager.RequestNavigate( RegionKeys.EditorDocumentRegion, assetReference.EditorKey, parameters );
    }

    private static void CloseTab( IAssetReference assetReference )
    {
      var regionManager = _container.Resolve<IRegionManager>();
      var region = regionManager.Regions[ RegionKeys.EditorDocumentRegion ];

      var editorView = FindEditorTab( region, assetReference );
      if ( editorView is null )
        return;

      RemoveTabFromRegion( editorView, region );
    }

    private static void CloseAllTabs()
    {
      var regionManager = _container.Resolve<IRegionManager>();
      var region = regionManager.Regions[ RegionKeys.EditorDocumentRegion ];

      var tabs = region.Views.ToArray();
      foreach ( var tab in tabs )
        RemoveTabFromRegion( tab, region );
    }

    private static void CloseAllTabsButThis( IAssetReference assetReference )
    {
      var regionManager = _container.Resolve<IRegionManager>();
      var region = regionManager.Regions[ RegionKeys.EditorDocumentRegion ];

      var tabs = region.Views.ToArray();
      foreach ( FrameworkElement tab in tabs )
      {
        if ( tab.DataContext is IEditorViewModel editorViewModel )
          if ( editorViewModel.AssetReference.Equals( assetReference ) )
            continue;

        RemoveTabFromRegion( tab, region );
      }
    }

    #endregion

    #region Tab Methods

    private static object FindEditorTab( IRegion region, IAssetReference assetReference )
    {
      foreach ( var view in region.Views )
      {
        var editorView = view as FrameworkElement;
        if ( editorView is null )
          continue;

        var editorViewModel = editorView.DataContext as IEditorViewModel;
        if ( editorViewModel is null )
          continue;

        if ( !editorViewModel.AssetReference.Equals( assetReference ) )
          continue;

        return editorView;
      }

      return null;
    }

    private static void RemoveTabFromRegion( object tab, IRegion region )
    {
      var navigationContext = new NavigationContext( region.NavigationService, null );
      if ( !CanRemoveTabFromRegion( tab, navigationContext ) )
        return;

      InvokeOnNavigatedFrom( tab, navigationContext );
      region.Remove( tab );

      var view = tab as FrameworkElement;
      if ( view is null )
        return;

      var disposeTasks = new Task[ 2 ];
      disposeTasks[ 0 ] = Task.CompletedTask;
      disposeTasks[ 1 ] = Task.CompletedTask;

      if ( tab is IDisposable disposableTab )
        disposeTasks[ 0 ] = Task.Run( () => disposableTab?.Dispose() );

      if ( view.DataContext is IDisposable disposableViewModel )
        disposeTasks[ 1 ] = Task.Run( () => disposableViewModel.Dispose() );

      Task.WhenAll( disposeTasks ).ContinueWith( t => { GCHelper.ForceCollect(); } );
    }

    private static bool CanRemoveTabFromRegion( object item, NavigationContext navigationContext )
    {
      var canRemove = true;

      var confirmRequestItem = item as IConfirmNavigationRequest;
      if ( confirmRequestItem is null )
      {
        var frameworkElement = item as FrameworkElement;
        if ( frameworkElement is not null )
          confirmRequestItem = frameworkElement.DataContext as IConfirmNavigationRequest;
      }

      if ( confirmRequestItem is not null )
      {
        confirmRequestItem.ConfirmNavigationRequest( navigationContext, result =>
        {
          canRemove = result;
        } );
      }

      return canRemove;
    }

    private static void InvokeOnNavigatedFrom( object item, NavigationContext navigationContext )
    {
      var navigationAware = item as INavigationAware;
      if ( navigationAware is null )
      {
        var frameworkElement = item as FrameworkElement;
        navigationAware = frameworkElement.DataContext as INavigationAware;
      }

      if ( navigationAware is not null )
        navigationAware.OnNavigatedFrom( navigationContext );
    }

    #endregion

    #region Export Methods

    private static void ExportAsset( IExportableAsset asset )
    {
      var optionsType = asset.ExportOptionsType;
      var jobType = asset.ExportJobType;

      var parameters = new DialogParameters();
      parameters.Add( "Asset", asset );
      parameters.Add( "AssetReference", asset.AssetReference );
      parameters.Add( "OptionsType", optionsType );
      parameters.Add( "JobType", jobType );

      var dialogService = _container.Resolve<IDialogService>();
      dialogService.ShowDialog( optionsType.Name, parameters, result =>
      {
        if ( result is null || result.Result != ButtonResult.OK )
          return;

        var options = result.Parameters.GetValue<object>( "Options" );

        var jobParams = new ParameterCollection();
        jobParams.Set<IAsset>( "Asset", asset );
        jobParams.Set( asset.AssetReference );
        jobParams.Set( "Options", options );

        var jobManager = _container.Resolve<IJobManager>();
        var job = jobManager.CreateJob( jobType, jobParams );
        jobManager.StartJob( job );
      } );

    }

    #endregion

  }

}
