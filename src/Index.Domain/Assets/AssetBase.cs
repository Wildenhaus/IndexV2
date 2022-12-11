namespace Index.Domain.Assets
{

  public abstract class AssetBase : DisposableObject, IAsset
  {

    #region Properties

    public abstract string TypeName { get; }
    public abstract string EditorKey { get; }

    public string AssetName => AssetReference.AssetName;
    public IAssetReference AssetReference { get; set; }
    public IAssetLoadContext AssetLoadContext { get; set; }

    #endregion

    #region Constructor

    protected AssetBase( IAssetReference assetReference )
    {
      AssetReference = assetReference;
    }

    #endregion

  }

}
