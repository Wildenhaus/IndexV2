using Index.Jobs;

namespace Index.Domain.Assets
{

  public interface IAssetFactory
  {

    IJob<IAsset> LoadAsset( IAssetReference assetReference );

  }

  public interface IAssetFactory<out TAsset> : IAssetFactory
    where TAsset : class, IAsset
  {

    new IJob<TAsset> LoadAsset( IAssetReference assetReference );

    IJob<IAsset> IAssetFactory.LoadAsset( IAssetReference assetReference )
      => LoadAsset( assetReference );

  }

}