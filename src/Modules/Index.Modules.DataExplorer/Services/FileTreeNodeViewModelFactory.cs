using System.Collections.Generic;
using System.Linq;
using Index.Domain.FileSystem;
using Index.Modules.DataExplorer.ViewModels;

namespace Index.Modules.DataExplorer.Services
{

  public class FileTreeNodeViewModelFactory
  {

    #region Data Members

    private IFileSystem _fileSystem;

    #endregion

    #region Constructor

    public FileTreeNodeViewModelFactory( IFileSystem fileSystem )
    {
      _fileSystem = fileSystem;
    }

    #endregion

    #region Public Methods

    public List<FileTreeNodeViewModel> CreateNodes()
    {
      var rootNodes = new List<FileTreeNodeViewModel>();

      foreach ( var device in _fileSystem.Devices.Values )
        rootNodes.Add( CreateNode( device.Root ) );

      return rootNodes;
    }

    #endregion

    #region Private Methods

    private FileTreeNodeViewModel CreateNode( IFileSystemNode fileSystemNode )
    {
      var viewModel = new FileTreeNodeViewModel( fileSystemNode );

      foreach ( var child in fileSystemNode.EnumerateChildren( recursive: false ).Where(x => !x.IsHidden) )
        viewModel.Children.Add( CreateNode( child ) );

      return viewModel;
    }

    #endregion

  }

}
