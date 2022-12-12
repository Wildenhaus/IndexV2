using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.Jobs;
using Index.Jobs;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class LoadGeometryTexturesJob : JobBase
  {

    private IAssetManager AssetManager { get; }
    private IAssetLoadContext AssetLoadContext { get; }

    public LoadGeometryTexturesJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      AssetManager = container.Resolve<IAssetManager>();
      AssetLoadContext = Parameters.Get<IAssetLoadContext>();
    }

    protected override async Task OnExecuting()
    {
      SetStatus( "Loading Textures" );
      SetIndeterminate();
      SetIncludeUnitsInStatus();

      var toLoadSet = GatherTextures();
      var textures = await LoadTextures( toLoadSet );

      Parameters.Set( "Textures", textures );
    }

    private HashSet<IAssetReference> GatherTextures()
    {
      var textureList = Parameters.Get<TextureList>();

      var toLoadSet = new HashSet<IAssetReference>();
      var textureAssetReferences = AssetManager.GetAssetReferencesOfType<ITextureAsset>();

      foreach ( var textureName in textureList )
      {
        var matches = textureAssetReferences.Where( x => x.AssetName.StartsWith( textureName ) );
        foreach ( var match in matches )
          toLoadSet.Add( match );
      }

      return toLoadSet;
    }

    private async Task<Dictionary<string, ITextureAsset>> LoadTextures( ICollection<IAssetReference> toLoadSet )
    {
      SetCompletedUnits( 0 );
      SetTotalUnits( toLoadSet.Count );
      SetIndeterminate( false );

      var loadedTextures = new Dictionary<string, ITextureAsset>();

      var jobs = new List<Task>();
      var jobSemaphore = new SemaphoreSlim( 5 );
      foreach ( var assetToLoad in toLoadSet )
      {
        jobSemaphore.Wait();
        var loadJob = AssetManager.LoadAsset<ITextureAsset>( assetToLoad, AssetLoadContext );
        loadJob.RegisterCompletionCallback( job =>
        {
          lock ( loadedTextures )
          {
            var texture = loadJob.Result;
            loadedTextures.Add( texture.AssetName, texture );
            IncreaseCompletedUnits( 1 );
            jobSemaphore.Release();
          }
        } );
        jobs.Add( loadJob.Completion );
      }

      await Task.WhenAll( jobs );
      return loadedTextures;
    }

  }

}
