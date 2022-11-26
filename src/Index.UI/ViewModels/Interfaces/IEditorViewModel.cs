using Index.Domain.Assets;

namespace Index.UI.ViewModels
{

  public interface IEditorViewModel<TAsset> : ITabViewModel, IInitializableViewModel
    where TAsset : IAsset
  {

    #region Properties

    public TAsset Asset { get; }
    public IAssetReference AssetReference { get; }

    #endregion

  }

}