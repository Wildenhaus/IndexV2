using Index.Jobs;

namespace Index.Domain.Assets
{

  public interface IAssetLoadContext : IDisposable
  {

    #region Properties

    IReadOnlyDictionary<IAssetReference, IJob> LoadingAssets { get; }
    IReadOnlyDictionary<IAssetReference, IAsset> LoadedAssets { get; }

    #endregion

    #region Public Methods

    void AddAsset( IAsset asset );

    bool TryGetAsset( IAssetReference assetReference, out IAsset asset );
    bool TryGetAsset<TAsset>( IAssetReference assetReference, out TAsset asset )
      where TAsset : IAsset;

    IJob<TAsset> GetAssetLoadingJob<TAsset>( IAssetReference assetReference ) where TAsset : IAsset;
    bool TryGetAssetLoadingJob<TAsset>( IAssetReference assetReference, out IJob<TAsset> assetLoadJob ) where TAsset : IAsset;

    void MarkAssetAsLoading<TAsset>( IAssetReference assetReference, IJob<TAsset> loadAssetJob ) where TAsset : IAsset;
    void MarkAssetAsFinishedLoading<TAsset>( IAssetReference assetReference ) where TAsset : IAsset;

    #endregion

  }

}
