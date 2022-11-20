using Index.Domain.FileSystem;

namespace Index.Domain.Assets
{

  public class AssetManager : IAssetManager
  {

    #region Data Members

    private readonly Dictionary<Type, IAssetReferenceCollection> _references;

    private bool _isInitialized;

    #endregion

    #region Properties

    public IEnumerable<IAssetReferenceCollection> ReferenceCollections
    {
      get => _references.Values;
    }

    #endregion

    #region Constructor

    public AssetManager()
    {
      _references = new Dictionary<Type, IAssetReferenceCollection>();
    }

    #endregion

    #region Public Methods

    public void InitializeFromFileSystem( IFileSystem fileSystem )
    {
      if ( _isInitialized )
        return;

      foreach ( var device in fileSystem.Devices.Values )
      {
        var assetNodes = device.EnumerateFiles().OfType<IFileSystemAssetNode>();
        foreach ( var assetNode in assetNodes )
          AddAssetReference( new AssetReference( assetNode ) );
      }

      _isInitialized = true;
    }

    public void AddAssetReference( IAssetReference assetReference )
    {
      var assetType = assetReference.AssetType;
      var referenceCollection = GetReferenceCollection( assetType );

      referenceCollection.AddReference( assetReference );
    }

    public bool TryGetAssetReference( Type assetType, string assetName, out IAssetReference assetReference )
    {
      assetReference = default;

      if ( !_references.TryGetValue( assetType, out var referenceCollection ) )
        return false;

      return referenceCollection.TryGetReference( assetName, out assetReference );
    }

    #endregion

    #region Private Methods

    private IAssetReferenceCollection GetReferenceCollection( Type assetType )
    {
      if ( _references.TryGetValue( assetType, out var referenceCollection ) )
        return referenceCollection;

      ASSERT( assetType.IsAssignableTo( typeof( IAsset ) ), "`{0}` is not an IAsset.", assetType.FullName );

      var referenceCollectionType = typeof( AssetReferenceCollection<> ).MakeGenericType( assetType );
      referenceCollection = ( IAssetReferenceCollection ) Activator.CreateInstance( referenceCollectionType );
      _references.Add( assetType, referenceCollection );

      return referenceCollection;
    }

    private IAssetReferenceCollection<TAsset> GetReferenceCollection<TAsset>()
      where TAsset : IAsset
      => GetReferenceCollection( typeof( TAsset ) ) as IAssetReferenceCollection<TAsset>;

    #endregion

  }

}
