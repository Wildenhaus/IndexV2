using Index.Domain.Jobs;
using Index.Jobs;

namespace Index.Domain.Assets
{

  public class AssetLoadContext : DisposableObject, IAssetLoadContext
  {

    #region Data Members

    private readonly Dictionary<IAssetReference, IJob> _loadingAssets;
    private readonly Dictionary<IAssetReference, IAsset> _loadedAssets;

    #endregion

    #region Properties

    public IReadOnlyDictionary<IAssetReference, IJob> LoadingAssets => _loadingAssets;
    public IReadOnlyDictionary<IAssetReference, IAsset> LoadedAssets => _loadedAssets;

    #endregion

    #region Constructor

    public AssetLoadContext()
    {
      _loadingAssets = new Dictionary<IAssetReference, IJob>();
      _loadedAssets = new Dictionary<IAssetReference, IAsset>();
    }

    #endregion

    #region Public Methods

    public void AddAsset( IAsset asset )
    {
      lock ( _loadedAssets )
        _loadedAssets.Add( asset.AssetReference, asset );
    }

    public bool TryGetAsset( IAssetReference assetReference, out IAsset asset )
    {
      lock ( _loadedAssets )
        return _loadedAssets.TryGetValue( assetReference, out asset );
    }

    public bool TryGetAsset<TAsset>( IAssetReference assetReference, out TAsset asset )
      where TAsset : IAsset
    {
      asset = default;

      if ( !TryGetAsset( assetReference, out var untypedAsset ) )
        return false;

      asset = ( TAsset ) untypedAsset;
      return true;
    }

    public IJob<TAsset> GetAssetLoadingJob<TAsset>(IAssetReference assetReference )
      where TAsset : IAsset
    {
      if(!TryGetAssetLoadingJob<TAsset>(assetReference, out var assetLoadJob))
        return FAIL_RETURN<IJob<TAsset>>( "Asset is not marked as loaded." );

      return assetLoadJob;
    }

    public bool TryGetAssetLoadingJob<TAsset>( IAssetReference assetReference, out IJob<TAsset> assetLoadJob )
      where TAsset : IAsset
    {
      assetLoadJob = default;

      lock ( _loadingAssets )
      {
        if ( !_loadingAssets.TryGetValue( assetReference, out var job ) )
        {
          if ( !TryGetAsset<TAsset>( assetReference, out var asset ) )
            return false;
          else
          {
            assetLoadJob = new CompletedJob<TAsset>( asset );
            return true;
          }
        }
        else
        {
          assetLoadJob = job as IJob<TAsset>;
          return true;
        }
      }
    }

    public void MarkAssetAsLoading<TAsset>(IAssetReference assetReference, IJob<TAsset> loadAssetJob)
      where TAsset : IAsset
    {
      lock(_loadingAssets)
      {
        _loadingAssets.Add( assetReference, loadAssetJob );
      }
    }

    public void MarkAssetAsFinishedLoading<TAsset>(IAssetReference assetReference)
      where TAsset : IAsset
    {
      lock( _loadingAssets)
      {
        if ( !_loadingAssets.TryGetValue( assetReference, out var assetLoadJob ) )
          FAIL( "Asset was not marked as loading." );

        _loadingAssets.Remove(assetReference);
      }
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      foreach ( var asset in _loadedAssets.Values )
        asset?.Dispose();
    }

    #endregion

  }
}
