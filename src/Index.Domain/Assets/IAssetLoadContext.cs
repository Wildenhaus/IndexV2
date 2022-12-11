namespace Index.Domain.Assets
{

  public interface IAssetLoadContext : IDisposable
  {

    #region Properties

    IReadOnlyDictionary<IAssetReference, IAsset> LoadedAssets { get; }

    #endregion

    #region Public Methods

    void AddAsset( IAsset asset );

    bool TryGetAsset( IAssetReference assetReference, out IAsset asset );
    bool TryGetAsset<TAsset>( IAssetReference assetReference, out TAsset asset )
      where TAsset : IAsset;

    #endregion

  }

}
