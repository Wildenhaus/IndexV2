using Index.Domain.FileSystem;

namespace Index.Modules.FileExplorer.ViewModels
{

  public class FileExplorerViewModel
  {

    #region Data Members

    private IFileSystem _fileSystem;

    #endregion

    #region Constructor

    public FileExplorerViewModel( IFileSystem fileSystem )
    {
    }

    #endregion

  }

}
