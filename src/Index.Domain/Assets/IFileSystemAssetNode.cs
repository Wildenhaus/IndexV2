using Index.Domain.FileSystem;

namespace Index.Domain.Assets
{

  public interface IFileSystemAssetNode : IFileSystemNode
  {

    #region Properties

    Type AssetType { get; }
    Type AssetFactoryType { get; }

    #endregion

  }

  public interface IFileSystemAssetNode<TAsset> : IFileSystemAssetNode
    where TAsset : IAsset
  {

    #region Properties

    Type IFileSystemAssetNode.AssetType => typeof( TAsset );

    #endregion

  }

  public interface IFileSystemAssetNode<TAsset, TFactory> : IFileSystemAssetNode<TAsset>
    where TAsset : class, IAsset
    where TFactory : IAssetFactory<TAsset>
  {

    #region Properties

    Type IFileSystemAssetNode.AssetFactoryType => typeof( TFactory );

    #endregion

  }

}
