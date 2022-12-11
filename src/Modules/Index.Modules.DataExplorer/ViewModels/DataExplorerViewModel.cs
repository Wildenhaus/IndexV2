using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Domain.Models;
using Index.Modules.DataExplorer.Services;
using Index.Utilities;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using PropertyChanged;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class DataExplorerViewModel : BindableBase
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IFileSystem _fileSystem;
    private readonly IRegionManager _regionManager;

    private readonly ActionDebouncer _searchDebouncer;

    #endregion

    #region Properties

    public ObservableCollection<AssetNodeViewModel> AssetNodes { get; }
    public ObservableCollection<FileTreeNodeViewModel> FileTreeNodes { get; }

    [OnChangedMethod( nameof( OnSearchTermChanged ) )]
    public string SearchTerm { get; set; }

    public ICommand NavigateToAssetCommand { get; }

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

      _searchDebouncer = new ActionDebouncer( 1000, ApplySearchTerm );
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

    private void NavigateToAsset( IAssetReference assetReference )
    {
      var parameters = new NavigationParameters();
      parameters.Add( "AssetReference", assetReference );

      _regionManager.RequestNavigate( "EditorRegion", "TextureEditorView", parameters );
    }

    private void OnSearchTermChanged()
      => _searchDebouncer.Invoke();

    private void ApplySearchTerm()
    {
      var searchTerm = SearchTerm.ToLower();
      var searchTask = Task.Factory.StartNew( () =>
      {
        foreach ( var node in AssetNodes )
          node.ApplySearchCriteria( searchTerm );
      }, TaskCreationOptions.LongRunning );
      searchTask.Wait();
    }

    #endregion

  }

}
