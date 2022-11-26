using System.Threading.Tasks;
using Index.Domain.Assets;
using Prism.Ioc;
using Prism.Regions;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  public abstract class EditorViewModelBase<TAsset> : InitializableViewModel, IEditorViewModel<TAsset>
    where TAsset : IAsset
  {

    #region Properties

    public string TabName { get; set; }

    [DoNotNotify] public TAsset Asset { get; protected set; }
    [DoNotNotify] public IAssetReference AssetReference { get; protected set; }

    [DoNotNotify] protected IContainerProvider Container { get; }
    [DoNotNotify] protected IAssetManager AssetManager { get; }

    #endregion

    #region Constructor

    protected EditorViewModelBase( IContainerProvider container )
    {
      Container = container;
      AssetManager = container.Resolve<IAssetManager>();
    }

    #endregion

    #region Overrides

    protected override async Task<StatusList> OnInitializing()
    {
      Asset = await AssetManager.LoadAsset<TAsset>( AssetReference );
      await OnAssetLoaded( Asset );
      return new StatusList();
    }

    public override bool IsNavigationTarget( NavigationContext navigationContext )
    {
      var assetReference = ( IAssetReference ) navigationContext.Parameters[ "AssetReference" ];
      return assetReference == AssetReference;
    }

    public override void OnNavigatedTo( NavigationContext navigationContext )
    {
      if ( AssetReference is null )
      {
        AssetReference = ( IAssetReference ) navigationContext.Parameters[ "AssetReference" ];
        TabName = AssetReference.AssetName;
        Initialize();
      }
    }

    #endregion

    #region Abstract Methods

    protected abstract Task OnAssetLoaded( TAsset asset );

    #endregion

  }

}
