using Index.Domain.Assets;
using Prism.Ioc;
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

    protected EditorViewModelBase( IAssetReference assetReference, IContainerProvider container )
    {
      AssetReference = assetReference;
      Container = container;
      AssetManager = container.Resolve<IAssetManager>();

      TabName = assetReference.AssetName;
    }

    #endregion

  }

}
