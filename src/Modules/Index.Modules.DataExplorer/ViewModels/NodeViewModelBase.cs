using System.Collections.ObjectModel;
using System.Windows.Controls;
using Index.UI.Controls.Menus;
using Prism.Mvvm;

namespace Index.Modules.DataExplorer.ViewModels
{

  public abstract class NodeViewModelBase<TViewModel> : BindableBase
    where TViewModel : NodeViewModelBase<TViewModel>
  {

    #region Data Members

    private readonly ObservableCollection<TViewModel> _children;

    private bool _isSelected;
    private bool _isExpanded;
    private bool _isVisible;

    #endregion

    #region Properties

    public ObservableCollection<TViewModel> Children
    {
      get => _children;
    }

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
      _children = new ObservableCollection<TViewModel>();
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

    #endregion

  }

}
