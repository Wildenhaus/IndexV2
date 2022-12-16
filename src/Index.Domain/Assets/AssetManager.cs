using Index.Domain.FileSystem;
using Index.Domain.Jobs;
using Index.Jobs;
using Prism.Ioc;

namespace Index.Domain.Assets
{

  public class AssetManager : IAssetManager
  {

    #region Data Members

    private readonly IContainerProvider _container;
    private readonly IJobManager _jobManager;

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

    public AssetManager( IContainerProvider container )
    {
      _container = container;
      _jobManager = _container.Resolve<IJobManager>();

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

    public IJob<TAsset> LoadAsset<TAsset>( IAssetReference assetReference, IAssetLoadContext loadContext = null )
      where TAsset : class, IAsset
    {
      ASSERT_NOT_NULL( assetReference );
      ASSERT( assetReference.AssetType.IsAssignableTo( typeof( TAsset ) ), $"Asset type mismatch." );

      if ( loadContext is not null )
      {
        if ( loadContext.TryGetAsset<TAsset>( assetReference, out var loadedAsset ) )
          return new CompletedJob<TAsset>( loadedAsset );
      }

      var factoryType = assetReference.AssetFactoryType;
      var factory = ( IAssetFactory<TAsset> ) _container.Resolve( factoryType );

      var loadAssetJob = factory.LoadAsset( assetReference );
      Task.WhenAny( loadAssetJob.Completion ).ContinueWith( t =>
      {
        if ( loadAssetJob.State != JobState.Completed )
          return;

        if ( loadContext is not null )
        {
          loadAssetJob.Result.AssetLoadContext = loadContext;
          loadContext.AddAsset( loadAssetJob.Result );
        }
      } );
      return loadAssetJob;
    }

    public async Task<TAsset> LoadAssetAsync<TAsset>( IAssetReference assetReference, IAssetLoadContext loadContext = null )
      where TAsset : class, IAsset
    {
      var loadJob = LoadAsset<TAsset>( assetReference, loadContext );
      await loadJob.Completion;

      return loadJob.Result;
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

      var referenceCollections = _references.Where( x => x.Key.IsAssignableTo( assetType ) );
      foreach ( var referenceCollection in referenceCollections )
      {
        if ( referenceCollection.Value.TryGetReference( assetName, out assetReference ) )
          return true;
      }

      return false;
    }

    public IEnumerable<IAssetReference> GetAssetReferencesOfType<TAsset>()
    {
      foreach ( var referenceCollection in _references )
      {
        if ( !referenceCollection.Key.IsAssignableTo( typeof( TAsset ) ) )
          continue;

        foreach ( var assetReference in referenceCollection.Value )
          yield return assetReference;
      }
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
