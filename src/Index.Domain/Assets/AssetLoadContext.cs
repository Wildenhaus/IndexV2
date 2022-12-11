namespace Index.Domain.Assets
{

  public class AssetLoadContext : DisposableObject, IAssetLoadContext
  {

    #region Data Members

    private readonly Dictionary<IAssetReference, IAsset> _loadedAssets;

    #endregion

    #region Properties

    public IReadOnlyDictionary<IAssetReference, IAsset> LoadedAssets => _loadedAssets;

    #endregion

    #region Constructor

    public AssetLoadContext()
    {
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
