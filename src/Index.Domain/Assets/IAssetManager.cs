using Index.Domain.FileSystem;
using Index.Jobs;

namespace Index.Domain.Assets
{

  public interface IAssetManager
  {

    #region Properties

    IEnumerable<IAssetReferenceCollection> ReferenceCollections { get; }

    #endregion

    #region Public Methods

    void InitializeFromFileSystem( IFileSystem fileSystem );

    IJob<TAsset> LoadAsset<TAsset>( IAssetReference assetReference )
      where TAsset : class, IAsset;

    void AddAssetReference( IAssetReference assetReference );
    bool TryGetAssetReference( Type assetType, string assetName, out IAssetReference assetReference );
    IEnumerable<IAssetReference> GetAssetReferencesOfType<TAsset>();

    #endregion

  }

}
