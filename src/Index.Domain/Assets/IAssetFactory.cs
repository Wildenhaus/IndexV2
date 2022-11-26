namespace Index.Domain.Assets
{

  public interface IAssetFactory<TAsset>
    where TAsset : IAsset
  {

    Task<TAsset> LoadAsset( IAssetReference assetReference );

  }

}