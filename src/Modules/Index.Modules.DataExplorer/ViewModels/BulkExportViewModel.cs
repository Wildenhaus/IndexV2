using System;
using System.Collections.Generic;
using System.Linq;
using Index.Domain;
using Index.Domain.Assets;
using Index.Domain.Editors;
using Index.Domain.FileSystem;
using Index.Modules.DataExplorer.Services;
using Index.UI.Common;
using Index.UI.ViewModels;
using Index.UI.Views;
using Prism.Ioc;
using Prism.Regions;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class BulkExportViewModel : TabViewModelBase
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IContainerProvider _container;
    private readonly IFileSystem _fileSystem;
    private readonly IRegionManager _regionManager;

    #endregion

    #region Properties

    public IRegionManager RegionManager => _regionManager;

    public IxObservableCollection<BulkExportAssetNodeViewModel> ExportNodes { get; private set; }
    public Dictionary<Type, IAssetExportOptionsTabViewModel> ExportOptions { get; }

    #endregion

    #region Constructor

    public BulkExportViewModel( IContainerProvider containerProvider )
    {
      TabName = "Bulk Export";
      ExportOptions = new Dictionary<Type, IAssetExportOptionsTabViewModel>();

      _container = containerProvider;
      _assetManager = containerProvider.Resolve<IAssetManager>();
      _fileSystem = containerProvider.Resolve<IFileSystem>();
      _regionManager = containerProvider.Resolve<RegionManager>();
    }

    #endregion

    #region Overrides

    public override bool IsNavigationTarget( NavigationContext navigationContext )
    {
      return navigationContext.Uri.ToString() == DefaultEditorKeys.BulkExport;
    }

    public override void OnNavigatedTo( NavigationContext navigationContext )
    {
      if ( ExportNodes is null )
      {
        var assetNodes = navigationContext.Parameters[ "AssetNodes" ] as ICollection<AssetNodeViewModel>;
        InitializeNodes( assetNodes );
      }
    }

    #endregion

    #region Private Methods

    private void InitializeNodes( ICollection<AssetNodeViewModel> nodes )
    {
      var factory = new BulkExportAssetNodeViewModelFactory();
      ExportNodes = factory.CreateExportNodes( nodes );

      foreach ( var node in ExportNodes )
      {
        InitializeOptions( node.AssetType );
        node.PropertyChanged += OnRootNodePropertyChanged;
      }
    }

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

          tabRegion.Add( optionsView, optionsViewModel.AssetTypeName );
        }
      }
      else if (node.IsChecked == false)
      {
        if ( existingTab is null )
          return;

        tabRegion.Remove( existingTab );
      }
    }

    #endregion

  }

}
