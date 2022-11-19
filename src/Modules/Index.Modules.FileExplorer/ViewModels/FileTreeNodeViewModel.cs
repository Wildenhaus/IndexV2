using System.Collections.Generic;
using System.Linq;
using Index.Domain.ViewModels;

namespace Index.Modules.FileExplorer.ViewModels
{

  public class FileTreeNodeViewModel : ViewModelBase
  {

    #region Data Members

    private readonly string _name;
    private readonly List<FileTreeNodeViewModel> _children;

    private bool _isExpanded;
    private bool _isVisible;

    #endregion

    #region Properties

    public string Name
    {
      get => _name;
    }

    public IList<FileTreeNodeViewModel> Children
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

    public FileTreeNodeViewModel( string name, IEnumerable<FileTreeNodeViewModel> children = null )
    {
      _name = name;
      _isExpanded = false;
      _isVisible = true;

      _children = new List<FileTreeNodeViewModel>( children ?? Enumerable.Empty<FileTreeNodeViewModel>() );
    }

    #endregion

  }

}
