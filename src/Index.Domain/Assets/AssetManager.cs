﻿using System.Reflection;
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
    private readonly Dictionary<Type, Type> _exportOptionsViewTypeMapping;

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
      _exportOptionsViewTypeMapping = new Dictionary<Type, Type>();
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

      if ( loadContext is null )
        loadContext = new AssetLoadContext();

      var factoryType = assetReference.AssetFactoryType;
      var factory = ( IAssetFactory<TAsset> ) _container.Resolve( factoryType );

      lock ( loadContext )
      {
        if ( loadContext.TryGetAsset<TAsset>( assetReference, out var loadedAsset ) )
          return new CompletedJob<TAsset>( loadedAsset );

        if ( loadContext.TryGetAssetLoadingJob<TAsset>( assetReference, out var existingLoadJob ) )
          return existingLoadJob;

        var loadAssetJob = factory.LoadAsset( assetReference, loadContext );
        loadContext.MarkAssetAsLoading( assetReference, loadAssetJob );

        Task.WhenAny( loadAssetJob.Completion ).ContinueWith( t =>
        {
          if ( loadAssetJob.State != JobState.Completed )
            return;

          loadAssetJob.Result.AssetLoadContext = loadContext;
          loadContext.AddAsset( loadAssetJob.Result );
          loadContext.MarkAssetAsFinishedLoading<TAsset>( assetReference );
        } );

        return loadAssetJob;
      }
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

    public string GetAssetTypeName( Type assetType )
    {
      if ( !_references.TryGetValue( assetType, out var referenceCollection ) )
        FAIL( "Asset Type not found: " + assetType.FullName );

      return referenceCollection.AssetTypeName;
    }

    public Type GetAssetExportOptionsType( Type assetType )
    {
      ASSERT_NOT_NULL( assetType );

      var attribute = assetType.GetCustomAttribute<AssetExportOptionsTypeAttribute>();
      ASSERT_NOT_NULL( attribute );

      var assetExportOptionsType = attribute.Type;
      ASSERT_NOT_NULL( assetExportOptionsType );

      return assetExportOptionsType;
    }

    public void RegisterViewTypeForExportOptionsType( Type exportOptionsType, Type viewType )
    {
      _exportOptionsViewTypeMapping.Add( exportOptionsType, viewType );
    }

    public Type GetViewTypeForExportOptionsType( Type exportOptionsType )
    {
      return _exportOptionsViewTypeMapping[ exportOptionsType ];
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
