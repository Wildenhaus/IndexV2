using Index.Domain.FileSystem;

namespace Index.Domain.Assets
{

  public interface IFileSystemAssetNode : IFileSystemNode
  {

    #region Properties

    Type AssetType { get; }

    #endregion

  }

  public interface IFileSystemAssetNode<TAsset> : IFileSystemAssetNode
    where TAsset : IAsset
  {

    #region Properties

    Type IFileSystemAssetNode.AssetType => typeof( TAsset );

    #endregion

  }

}
