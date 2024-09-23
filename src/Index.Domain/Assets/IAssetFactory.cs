using Index.Jobs;

namespace Index.Domain.Assets
{

  public interface IAssetFactory
  {

    IJob<IAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null );

  }

  public interface IAssetFactory<out TAsset> : IAssetFactory
    where TAsset : class, IAsset
  {

    new IJob<TAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null );

    IJob<IAsset> IAssetFactory.LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
      => LoadAsset( assetReference, assetLoadContext );

  }

}