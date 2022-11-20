using Index.Domain.FileSystem;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class FileTreeNodeViewModel : NodeViewModelBase<FileTreeNodeViewModel>
  {

    #region Data Members

    private IFileSystemNode _node;

    #endregion

    #region Properties

    public string Name
    {
      get => _node.DisplayName;
    }

    #endregion

    #region Constructor

    public FileTreeNodeViewModel( IFileSystemNode node )
    {
      _node = node;
    }

    #endregion

  }

}
