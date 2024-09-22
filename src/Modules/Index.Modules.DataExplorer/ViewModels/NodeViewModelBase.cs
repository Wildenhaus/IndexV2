using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using Index.UI.Common;
using Index.UI.Controls.Menus;
using Prism.Mvvm;

namespace Index.Modules.DataExplorer.ViewModels
{

  public abstract class NodeViewModelBase<TViewModel> : BindableBase
    where TViewModel : NodeViewModelBase<TViewModel>
  {

    #region Data Members

    private readonly IxObservableCollection<TViewModel> _children;

    private bool _isSelected;
    private bool _isExpanded;
    private bool _isVisible;

    #endregion

    #region Properties

    public abstract string Name { get; }

    public IxObservableCollection<TViewModel> Children
    {
      get => _children;
    }

    public ICollectionView ChildrenView { get; }

    public bool IsExpanded
    {
      get => _isExpanded;
      set => SetProperty( ref _isExpanded, value );
    }

    public bool IsSelected
    {
      get => _isSelected;
      set => SetProperty( ref _isSelected, value );
    }

    public bool IsVisible
    {
      get => _isVisible;
      set => SetProperty( ref _isVisible, value );
    }

    public bool IsLeaf
    {
      get => _children.Count == 0;
    }

    public ICommand DoubleClickCommand { get; protected set; }

    public ContextMenu ContextMenu
    {
      get => BuildContextMenu();
    }

    #endregion

    #region Constructor

    public NodeViewModelBase()
    {
      _isExpanded = false;
      _isVisible = true;
      _children = new IxObservableCollection<TViewModel>();

      ChildrenView = CollectionViewSource.GetDefaultView( _children );
      ChildrenView.Filter = ( node ) =>
      {
        var vm = node as NodeViewModelBase<TViewModel>;
        return vm.IsVisible;
      };
    }

    #endregion

    #region Public Methods

    public bool ApplySearchCriteria( string searchTerm )
    {
      var isEmpty = string.IsNullOrEmpty( searchTerm );

      if ( IsLeaf )
      {
        if ( isEmpty )
          return IsVisible = true;
        else
          return IsVisible = IsMatchForSearchTerm( searchTerm );
      }
      else
      {
        var childrenVisible = false;

        foreach ( var child in _children )
          childrenVisible |= child.ApplySearchCriteria( searchTerm );

        if ( isEmpty )
          childrenVisible = true;

        IsExpanded = !isEmpty && childrenVisible;
        IsVisible = childrenVisible;
        return childrenVisible;
      }
    }

    #endregion

    #region Private Methods

    private ContextMenu BuildContextMenu()
    {
      var builder = new MenuViewModelBuilder();
      OnConfigureContextMenu( builder );

      if ( builder.Items.Count == 0 )
        return null;

      var menu = new ContextMenu();
      menu.ItemsSource = builder.Items;

      return menu;
    }

    protected virtual void OnConfigureContextMenu( MenuViewModelBuilder builder )
    {
    }

    [MethodImpl( MethodImplOptions.AggressiveInlining )]
    protected virtual bool IsMatchForSearchTerm( string searchTerm )
      => Name.Contains( searchTerm, StringComparison.OrdinalIgnoreCase );

    #endregion

  }

}
