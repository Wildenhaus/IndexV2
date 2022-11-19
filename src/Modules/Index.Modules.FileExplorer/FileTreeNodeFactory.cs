using System.Collections.Generic;
using Index.Domain.FileSystem;
using Index.Modules.FileExplorer.ViewModels;

namespace Index.Modules.FileExplorer
{

  public class FileTreeNodeFactory
  {

    private IFileSystem _fileSystem;

    public FileTreeNodeFactory( IFileSystem fileSystem )
    {
      _fileSystem = fileSystem;
    }

    public List<FileTreeNodeViewModel> CreateNodes()
    {
      var roots = new List<FileTreeNodeViewModel>();

      var root = new FileTreeNodeViewModel( "Root" );
      roots.Add( root );
      foreach ( var node in _fileSystem.EnumerateFiles() )
      {
        root.Children.Add( new FileTreeNodeViewModel( node.GetPath() ) );
      }

      return roots;
    }

  }

}
