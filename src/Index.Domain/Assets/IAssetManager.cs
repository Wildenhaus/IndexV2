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

    void AddAssetReference( IAssetReference assetReference );
    bool TryGetAssetReference( Type assetType, string assetName, out IAssetReference assetReference );

    #endregion

  }

}
