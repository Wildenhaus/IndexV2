using Index.Domain.Assets;
using Index.Domain.Editors;
using Index.Domain.Models;
using Index.Domain;
using Index.Modules.DataExplorer.Services;
using Index.Modules.DataExplorer.ViewModels;
using Index.UI.Common;
using Prism.Regions;
using System.Collections.Generic;
using System.Windows.Input;
using Index.UI.Commands;
using Prism.Commands;

namespace Index.Modules.DataExplorer.ViewModels;

public class DataExplorerViewModel : SearchableNodeGraphViewModel<AssetNodeViewModel>
{

  #region Data Members

  private readonly IAssetManager _assetManager;
  private readonly IRegionManager _regionManager;

  #endregion

  #region Properties

  public ICommand OpenAboutDialogCommand { get; }
  public ICommand OpenBulkExportCommand { get; }

  #endregion

  public DataExplorerViewModel( IRegionManager regionManager, IEditorEnvironment environment )
  {
    _regionManager = regionManager;
    _assetManager = environment.AssetManager;

    OpenAboutDialogCommand = GlobalCommands.OpenAboutDialogCommand;
    OpenBulkExportCommand = new DelegateCommand( OpenBulkExport );

    // Initialize nodes and search graph
    Initialize();
  }

  protected override IxObservableCollection<AssetNodeViewModel> InitializeNodes()
  {
    var factory = new AssetNodeViewModelFactory( _assetManager );
    return new IxObservableCollection<AssetNodeViewModel>( factory.CreateNodes() );
  }

  protected override List<AssetNodeViewModel> InitializeSearchGraph( IxObservableCollection<AssetNodeViewModel> nodes )
  {
    var factory = new AssetNodeViewModelFactory( _assetManager );
    return factory.CreateNodeSearchGraph( nodes );
  }

  private void NavigateToAsset( IAssetReference assetReference )
  {
    var parameters = new NavigationParameters();
    parameters.Add( "AssetReference", assetReference );

    _regionManager.RequestNavigate( "EditorRegion", "TextureEditorView", parameters );
  }

  private void OpenBulkExport()
  {
    var parameters = new NavigationParameters();
    parameters.Add( "AssetNodes", Nodes );

    _regionManager.RequestNavigate( RegionKeys.EditorDocumentRegion, DefaultEditorKeys.BulkExport, parameters );
  }
}
