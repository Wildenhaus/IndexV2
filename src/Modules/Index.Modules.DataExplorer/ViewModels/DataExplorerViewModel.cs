using System.Collections.ObjectModel;
using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Domain.Models;
using Index.Domain.ViewModels;
using Index.Modules.DataExplorer.Services;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class DataExplorerViewModel : ViewModelBase
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IFileSystem _fileSystem;

    #endregion

    #region Properties

    public ObservableCollection<AssetNodeViewModel> AssetNodes { get; }
    public ObservableCollection<FileTreeNodeViewModel> FileTreeNodes { get; }

    #endregion

    #region Constructor

    public DataExplorerViewModel( IEditorEnvironment environment )
    {
      _assetManager = environment.AssetManager;
      _fileSystem = environment.FileSystem;

      AssetNodes = InitializeAssetNodes( environment.AssetManager );
      FileTreeNodes = InitializeFileTreeNodes( environment.FileSystem );
    }

    #endregion

    #region Private Methods

    private ObservableCollection<AssetNodeViewModel> InitializeAssetNodes( IAssetManager assetManager )
    {
      var factory = new AssetNodeViewModelFactory( assetManager );
      return new ObservableCollection<AssetNodeViewModel>( factory.CreateNodes() );
    }

    private ObservableCollection<FileTreeNodeViewModel> InitializeFileTreeNodes( IFileSystem fileSystem )
    {
      var factory = new FileTreeNodeViewModelFactory( fileSystem );
      return new ObservableCollection<FileTreeNodeViewModel>( factory.CreateNodes() );
    }

    #endregion

  }

}
