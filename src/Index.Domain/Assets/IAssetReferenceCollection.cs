namespace Index.Domain.Assets
{

  public interface IAssetReferenceCollection : IEnumerable<IAssetReference>
  {

    #region Properties

    Type AssetType { get; }
    string AssetTypeName { get; }

    int Count { get; }

    #endregion

    #region Public Methods

    void AddReference( IAssetReference assetReference );
    bool TryGetReference( string assetName, out IAssetReference reference );

    #endregion

  }

  public interface IAssetReferenceCollection<TAsset> : IAssetReferenceCollection
    where TAsset : IAsset
  {

    #region Properties

    Type IAssetReferenceCollection.AssetType => typeof( TAsset );

    #endregion

  }

}
