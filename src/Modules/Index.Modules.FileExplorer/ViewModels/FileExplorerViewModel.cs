using System.Collections.ObjectModel;
using Index.Domain.FileSystem;

namespace Index.Modules.FileExplorer.ViewModels
{

  public class FileExplorerViewModel
  {

    #region Data Members

    private IFileSystem _fileSystem;
    private ObservableCollection<FileTreeNodeViewModel> _files;

    #endregion

    public ObservableCollection<FileTreeNodeViewModel> Files
    {
      get => _files;
    }

    #region Constructor

    public FileExplorerViewModel( IFileSystem fileSystem )
    {
      var factory = new FileTreeNodeFactory( fileSystem );
      _files = new ObservableCollection<FileTreeNodeViewModel>( factory.CreateNodes() );
    }

    #endregion

  }

}
