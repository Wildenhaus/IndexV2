using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Domain.Models;
using Index.Modules.DataExplorer.Services;
using Index.UI.Commands;
using Index.UI.Common;
using Index.UI.ViewModels;
using Prism.Commands;
using Prism.Regions;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class DataExplorerViewModel : ViewModelBase
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IFileSystem _fileSystem;
    private readonly IRegionManager _regionManager;

    private string _searchTerm;
    private List<AssetNodeViewModel> _searchGraph;

    #endregion

    #region Properties

    public IxObservableCollection<AssetNodeViewModel> AssetNodes { get; private set; }
    public ICollectionView AssetNodesView { get; private set; }

    public ICommand SearchCommand { get; }
    public ICommand NavigateToAssetCommand { get; }
    public ICommand OpenAboutDialogCommand { get; }

    #endregion

    #region Constructor

    public DataExplorerViewModel( IRegionManager regionManager, IEditorEnvironment environment )
    {
      _regionManager = regionManager;
      _assetManager = environment.AssetManager;
      _fileSystem = environment.FileSystem;

      AssetNodes = InitializeAssetNodes( environment.AssetManager );
      AssetNodesView = CollectionViewSource.GetDefaultView( AssetNodes );

      NavigateToAssetCommand = new DelegateCommand<IAssetReference>( NavigateToAsset );
      OpenAboutDialogCommand = GlobalCommands.OpenAboutDialogCommand;
      SearchCommand = new DelegateCommand<string>( ApplySearchTerm );
    }

    #endregion

    #region Private Methods

    private IxObservableCollection<AssetNodeViewModel> InitializeAssetNodes( IAssetManager assetManager )
    {
      var factory = new AssetNodeViewModelFactory( assetManager );
      var nodes = factory.CreateNodes();
      _searchGraph = factory.CreateNodeSearchGraph( nodes );

      return new IxObservableCollection<AssetNodeViewModel>( nodes );
    }

    private void NavigateToAsset( IAssetReference assetReference )
    {
      var parameters = new NavigationParameters();
      parameters.Add( "AssetReference", assetReference );

      _regionManager.RequestNavigate( "EditorRegion", "TextureEditorView", parameters );
    }

    private async void ApplySearchTerm( string searchTerm )
    {
      await Task.Run( () =>
      {
        AssetNodes.SuppressNotifications = true;

        bool isEmpty = string.IsNullOrEmpty( searchTerm );
        bool changesMade = false;

        foreach ( var node in _searchGraph )
        {
          bool wasVisible = node.IsVisible;
          if ( isEmpty )
          {
            node.IsVisible = true;
          }
          else
          {
            node.IsVisible = node.Name.Contains( searchTerm, StringComparison.OrdinalIgnoreCase ) ||
                             node.Children.Any( x => x.IsVisible );
          }

          if ( node.IsVisible != wasVisible )
          {
            changesMade = true;
          }
        }
        AssetNodes.SuppressNotifications = false;

        Application.Current.Dispatcher.BeginInvoke( () =>
        {
          foreach ( var node in _searchGraph )
            if(!node.IsLeaf)
              node.ChildrenView.Refresh();
        } );
      } );
    }

    #endregion

  }

}
