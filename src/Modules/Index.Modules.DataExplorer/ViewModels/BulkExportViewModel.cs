using Index.Domain;
using Index.Domain.Assets;
using Index.Domain.Editors;
using Index.Jobs;
using Index.Modules.DataExplorer.Jobs;
using Index.Modules.DataExplorer.Services;
using Index.Modules.DataExplorer.ViewModels;
using Index.UI.Commands;
using Index.UI.Common;
using Index.UI.ViewModels;
using Index.UI.Views;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Windows.Input;

namespace Index.Modules.DataExplorer.ViewModels;

public class BulkExportViewModel : SearchableNodeGraphViewModel<BulkExportAssetNodeViewModel>, ITabViewModel
{

  #region Data Members

  private readonly IContainerProvider _container;
  private readonly IAssetManager _assetManager;
  private readonly IRegionManager _regionManager;
  private readonly IJobManager _jobManager;

  private ICollection<AssetNodeViewModel> _assetNodes;

  #endregion

  #region Properties

  public IRegionManager RegionManager => _regionManager;
  public Dictionary<Type, IAssetExportOptionsTabViewModel> ExportOptions { get; }

  public string TabName { get; set; }
  public ICommand CloseCommand { get; }
  public ICommand ExportCommand { get; private set; }

  public bool IsExporting { get; set; }
  public IJob ExportJob { get; set; }

  #endregion

  #region Constructor

  public BulkExportViewModel( IContainerProvider containerProvider )
  {
    _container = containerProvider;
    _assetManager = _container.Resolve<IAssetManager>();
    _regionManager = _container.Resolve<IRegionManager>();
    _jobManager = _container.Resolve<IJobManager>();

    // Create a new RegionManager specific to this instance
    _regionManager = _regionManager.CreateRegionManager();

    ExportOptions = new();

    TabName = "Bulk Export";
    CloseCommand = new DelegateCommand( CloseTab, () => !IsExporting );
  }

  #endregion

  #region Overrides

  protected override IxObservableCollection<BulkExportAssetNodeViewModel> InitializeNodes()
  {
    var factory = new BulkExportAssetNodeViewModelFactory();
    return factory.CreateExportNodes( _assetNodes );
  }

  protected override List<BulkExportAssetNodeViewModel> InitializeSearchGraph( IxObservableCollection<BulkExportAssetNodeViewModel> nodes )
  {
    var factory = new BulkExportAssetNodeViewModelFactory();
    return factory.CreateNodeSearchGraph( nodes );
  }

  protected override void OnInitializationComplete()
  {
    foreach ( var node in Nodes )
    {
      InitializeOptions( node.AssetType );
      node.PropertyChanged += OnRootNodePropertyChanged;
    }

    ExportCommand = new DelegateCommand( ExportAssets, () => ValidateOptions() );
  }

  public override bool IsNavigationTarget( NavigationContext navigationContext )
  {
    return navigationContext.Uri.ToString() == DefaultEditorKeys.BulkExport;
  }

  public override void OnNavigatedTo( NavigationContext navigationContext )
  {
    if ( Nodes == null )
    {
      _assetNodes = navigationContext.Parameters[ "AssetNodes" ] as ICollection<AssetNodeViewModel>;
      Initialize();
    }
  }

  #endregion

  #region Private Methods

  private void InitializeOptions( Type assetType )
  {
    if ( ExportOptions.ContainsKey( assetType ) )
      return;

    var optionsType = _assetManager.GetAssetExportOptionsType( assetType );
    var viewModelType = CreateAssetExportOptionsViewModelType( optionsType );
    var optionsViewModel = ( IAssetExportOptionsTabViewModel ) Activator.CreateInstance( viewModelType, new object[] { assetType, _container } );

    ExportOptions.Add( assetType, optionsViewModel );
  }

  private Type CreateAssetExportOptionsViewModelType( Type optionsType )
  {
    var genericType = typeof( AssetExportOptionsTabViewModel<> );
    return genericType.MakeGenericType( optionsType );
  }

  private void OnRootNodePropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
  {
    if ( e.PropertyName != nameof( BulkExportAssetNodeViewModel.IsChecked ) )
      return;

    var node = sender as BulkExportAssetNodeViewModel;
    var assetType = node.AssetType;

    var optionsViewModel = ExportOptions[ assetType ];
    var optionsTypeName = optionsViewModel.AssetExportOptionsType.Name;
    var assetTypeName = optionsViewModel.AssetTypeName;

    var tabRegion = _regionManager.Regions[ RegionKeys.BulkExportOptionsTabsRegion ];
    var existingTab = tabRegion.GetView( assetTypeName );

    if ( node.IsChecked == true || !node.IsChecked.HasValue )
    {
      if ( existingTab is null )
      {
        var optionsViewType = _assetManager.GetViewTypeForExportOptionsType( optionsViewModel.AssetExportOptionsType );
        var optionsView = ( AssetExportOptionsViewBase ) Activator.CreateInstance( optionsViewType );
        optionsView.DataContext = optionsViewModel;

        optionsViewModel.Options.PropertyChanged += OnOptionsPropertyChanged;
        tabRegion.Add( optionsView, optionsViewModel.AssetTypeName );
      }
    }
    else if ( node.IsChecked == false )
    {
      if ( existingTab is null )
        return;

      optionsViewModel.Options.PropertyChanged -= OnOptionsPropertyChanged;
      tabRegion.Remove( existingTab );
    }
  }

  private bool ValidateOptions()
  {
    var tabRegion = _regionManager.Regions[ RegionKeys.BulkExportOptionsTabsRegion ];
    foreach (var optionsVm in ExportOptions.Values)
    {
      var optionsTab = tabRegion.GetView( optionsVm.AssetTypeName ) as AssetExportOptionsViewBase;
      if ( optionsTab is null )
        continue;

      optionsVm.Options.Validate();
      if(!optionsVm.Options.IsValid )
      {
        tabRegion.Activate( optionsTab );
        return false;
      }
    }

    return true;
  }

  private void ExportAssets()
  {
    var parameters = new ParameterCollection();
    parameters.Set( "assetsToExport", GetAssetsToExport() );
    parameters.Set( "exportOptions", ExportOptions.Values.ToDictionary(x => x.AssetType, x => x.Options ) );

    var job = new BulkExportJob( _container, parameters );
    ExportJob = job;

    IsExporting = true;
    ( CloseCommand as DelegateCommand ).RaiseCanExecuteChanged();
    _jobManager.StartJob( job, _ =>
    {
      IsExporting = false;
      ExportJob = null;
      ( CloseCommand as DelegateCommand ).RaiseCanExecuteChanged();
    } );
  }

  private IEnumerable<IAssetReference> GetAssetsToExport()
  {
    IEnumerable<IAssetReference> GetAssetsToExportRecursive(BulkExportAssetNodeViewModel node)
    {
      var assetReference = node.AssetReference;
      if ( assetReference != null && node.IsChecked == true )
        yield return assetReference;

      foreach ( var childNode in node.Children)
        foreach(var asset in GetAssetsToExportRecursive(childNode))
          yield return asset;
    }

    foreach ( var node in Nodes )
      foreach ( var asset in GetAssetsToExportRecursive( node ) )
        yield return asset;
  }

  private void CloseTab()
  {
    EditorCommands.CloseTabByViewModelTypeCommand.Execute( GetType() );
  }

  #endregion

  #region Event Handlers

  private void OnOptionsPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
  {
    ( ExportCommand as DelegateCommand ).RaiseCanExecuteChanged();
  }

  #endregion

}
