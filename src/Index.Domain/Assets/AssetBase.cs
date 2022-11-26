namespace Index.Domain.Assets
{

  public abstract class AssetBase : IAsset
  {

    #region Properties

    public abstract string TypeName { get; }
    public abstract string EditorKey { get; }

    public string AssetName { get; set; }
    public IAssetReference AssetReference { get; set; }

    #endregion

    #region Constructor

    protected AssetBase( IAssetReference assetReference )
    {
      AssetReference = assetReference;
    }

    #endregion

  }

}
