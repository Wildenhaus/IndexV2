using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Index.UI.Common;
using Index.UI.ViewModels;
using Prism.Commands;

namespace Index.Modules.DataExplorer.ViewModels
{
  public abstract class SearchableNodeGraphViewModel<TNodeViewModel> : ViewModelBase
      where TNodeViewModel : NodeViewModelBase<TNodeViewModel>
  {
    #region Data Members

    private string _searchTerm;
    private List<TNodeViewModel> _searchGraph;

    #endregion

    #region Properties

    public IxObservableCollection<TNodeViewModel> Nodes { get; private set; }
    public ICollectionView NodesView { get; private set; }

    public ICommand SearchCommand { get; }

    #endregion

    #region Constructor

    protected SearchableNodeGraphViewModel()
    {
      SearchCommand = new DelegateCommand<string>( ApplySearchTerm );
    }

    #endregion

    #region Abstract Methods

    protected abstract IxObservableCollection<TNodeViewModel> InitializeNodes();
    protected abstract List<TNodeViewModel> InitializeSearchGraph( IxObservableCollection<TNodeViewModel> nodes );

    #endregion

    #region Public Methods

    public void Initialize()
    {
      Nodes = InitializeNodes();
      _searchGraph = InitializeSearchGraph( Nodes );
      NodesView = CollectionViewSource.GetDefaultView( Nodes );

      OnInitializationComplete();
    }

    #endregion

    #region Private Methods

    private async void ApplySearchTerm( string searchTerm )
    {
      await Task.Run( () =>
      {
        Nodes.SuppressNotifications = true;

        bool isEmpty = string.IsNullOrEmpty( searchTerm );
        bool changesMade = false;

        foreach ( var node in _searchGraph )
          node.Children.SuppressNotifications = true;

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

        foreach ( var node in _searchGraph )
          node.Children.SuppressNotifications = false;

        Nodes.SuppressNotifications = false;

        Application.Current.Dispatcher.BeginInvoke( () =>
        {
          foreach ( var node in _searchGraph )
            if ( !node.IsLeaf )
              node.ChildrenView.Refresh();
        } );
      } );
    }

    protected virtual void OnInitializationComplete()
    {
    }

    #endregion
  }
}
