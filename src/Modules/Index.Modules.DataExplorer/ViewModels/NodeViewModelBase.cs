using System.Collections.Generic;
using System.Collections.ObjectModel;
using Index.Domain.ViewModels;

namespace Index.Modules.DataExplorer.ViewModels
{

  public abstract class NodeViewModelBase<TViewModel> : ViewModelBase
    where TViewModel : NodeViewModelBase<TViewModel>
  {

    #region Data Members

    private readonly ObservableCollection<TViewModel> _children;

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

    public bool IsVisible
    {
      get => _isVisible;
      set => SetProperty( ref _isVisible, value );
    }

    public bool IsLeaf
    {
      get => _children.Count == 0;
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

  }

}
