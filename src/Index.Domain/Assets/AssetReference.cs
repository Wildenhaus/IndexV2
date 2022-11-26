namespace Index.Domain.Assets
{

  public class AssetReference : IAssetReference
  {

    #region Properties

    public string AssetName { get; }
    public Type AssetType { get; }
    public Type AssetFactoryType { get; }
    public IFileSystemAssetNode Node { get; }

    #endregion

    #region Constructor

    public AssetReference( IFileSystemAssetNode node, string assetName = null )
    {
      Node = node;
      AssetType = node.AssetType;
      AssetFactoryType = node.AssetFactoryType;
      AssetName = assetName ?? node.DisplayName;
    }

    public AssetReference( Type assetType, IFileSystemAssetNode node, string assetName = null )
    {
      Node = node;
      AssetType = assetType;
      AssetFactoryType = node.AssetFactoryType;
      AssetName = assetName ?? node.DisplayName;
    }

    #endregion

  }

}
