namespace Index.Domain.Assets
{

  public abstract class AssetBase : IAsset
  {

    #region Properties

    public abstract string TypeName { get; }
    public abstract string EditorKey { get; }

    public string AssetName { get; set; }

    #endregion

  }

}
