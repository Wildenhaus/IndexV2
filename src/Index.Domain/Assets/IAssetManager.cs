using Index.Domain.FileSystem;

namespace Index.Domain.Assets
{

  public interface IAssetManager
  {

    #region Properties

    IEnumerable<IAssetReferenceCollection> ReferenceCollections { get; }

    #endregion

    #region Public Methods

    void InitializeFromFileSystem( IFileSystem fileSystem );

    Task<TAsset> LoadAsset<TAsset>( IAssetReference assetReference )
      where TAsset : IAsset;

    void AddAssetReference( IAssetReference assetReference );
    bool TryGetAssetReference( Type assetType, string assetName, out IAssetReference assetReference );

    #endregion

  }

}
