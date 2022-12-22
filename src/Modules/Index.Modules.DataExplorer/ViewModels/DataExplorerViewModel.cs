using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Domain.Models;
using Index.Modules.DataExplorer.Services;
using Index.UI.Commands;
using Index.UI.ViewModels;
using Index.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PropertyChanged;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class DataExplorerViewModel : ViewModelBase
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IFileSystem _fileSystem;
    private readonly IRegionManager _regionManager;

    private readonly ActionThrottler _searchDebouncer;

    #endregion

    #region Properties

    public ObservableCollection<AssetNodeViewModel> AssetNodes { get; }
    public ObservableCollection<FileTreeNodeViewModel> FileTreeNodes { get; }

    [OnChangedMethod( nameof( OnSearchTermChanged ) )]
    public string SearchTerm { get; set; }

    public ICommand NavigateToAssetCommand { get; }
    public ICommand OpenAboutDialogCommand { get; }

    #endregion

    #region Constructor

    public DataExplorerViewModel( IRegionManager regionManager, IEditorEnvironment environment )
    {
      _regionManager = regionManager;
      _assetManager = environment.AssetManager;
      _fileSystem = environment.FileSystem;

      AssetNodes = InitializeAssetNodes( environment.AssetManager );
      FileTreeNodes = InitializeFileTreeNodes( environment.FileSystem );

      NavigateToAssetCommand = new DelegateCommand<IAssetReference>( NavigateToAsset );
      OpenAboutDialogCommand = GlobalCommands.OpenAboutDialogCommand;

      _searchDebouncer = new ActionThrottler( ApplySearchTerm, 500 );
    }

    #endregion

    protected override void OnDisposing()
    {
      base.OnDisposing();
      _searchDebouncer.Dispose();
    }

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

    private void NavigateToAsset( IAssetReference assetReference )
    {
      var parameters = new NavigationParameters();
      parameters.Add( "AssetReference", assetReference );

      _regionManager.RequestNavigate( "EditorRegion", "TextureEditorView", parameters );
    }

    private void OnSearchTermChanged()
      => _searchDebouncer.Execute();

    private void ApplySearchTerm()
    {
      var searchTerm = SearchTerm;
      foreach ( var node in AssetNodes )
        node.ApplySearchCriteria( searchTerm );
    }

    #endregion

  }

}
