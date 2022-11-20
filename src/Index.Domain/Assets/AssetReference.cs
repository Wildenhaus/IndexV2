namespace Index.Domain.Assets
{

  public class AssetReference : IAssetReference
  {

    #region Properties

    public string AssetName { get; }
    public Type AssetType { get; }
    public IFileSystemAssetNode Node { get; }

    #endregion

    #region Constructor

    public AssetReference( IFileSystemAssetNode node, string assetName = null )
    {
      Node = node;
      AssetType = node.AssetType;
      AssetName = assetName ?? node.DisplayName;
    }

    public AssetReference( Type assetType, IFileSystemAssetNode node, string assetName = null )
    {
      Node = node;
      AssetType = assetType;
      AssetName = assetName ?? node.DisplayName;
    }

    #endregion

  }

}
