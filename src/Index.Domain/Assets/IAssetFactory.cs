namespace Index.Domain.Assets
{

  public interface IAssetFactory
  {

    Task<IAsset> LoadAsset( IAssetReference assetReference );

  }

  public interface IAssetFactory<TAsset> : IAssetFactory
    where TAsset : IAsset
  {

    new Task<TAsset> LoadAsset( IAssetReference assetReference );

    async Task<IAsset> IAssetFactory.LoadAsset( IAssetReference assetReference )
    {
      return await LoadAsset( assetReference );
    }

  }

}