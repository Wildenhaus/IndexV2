using Index.Domain.Assets;

namespace Index.UI.ViewModels
{

  public interface IEditorViewModel : ITabViewModel, IInitializableViewModel
  {

    #region Properties

    public IAssetReference AssetReference { get; }

    #endregion

  }

  public interface IEditorViewModel<TAsset> : IEditorViewModel
    where TAsset : IAsset
  {

    #region Properties

    public TAsset Asset { get; }

    #endregion

  }

}