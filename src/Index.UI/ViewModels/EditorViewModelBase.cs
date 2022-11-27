using System.Threading.Tasks;
using System.Windows.Input;
using Index.Domain.Assets;
using Index.Domain.Jobs;
using Index.Jobs;
using Prism.Ioc;
using Prism.Regions;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  public abstract class EditorViewModelBase<TAsset> : InitializableViewModel, IEditorViewModel<TAsset>
    where TAsset : class, IAsset
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
      : base( container )
    {
      Container = container;
      AssetManager = container.Resolve<IAssetManager>();
    }

    #endregion

    #region Overrides

    protected override IJob CreateInitializationJob( IContainerProvider container )
      => AssetManager.LoadAsset<TAsset>( AssetReference );

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

  }

}
