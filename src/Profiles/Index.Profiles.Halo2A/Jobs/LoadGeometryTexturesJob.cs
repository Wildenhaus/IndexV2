using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.Jobs;
using Index.Jobs;
using Index.Profiles.Halo2A.Meshes;
using LibSaber.Halo2A.Serialization.Scripting;
using LibSaber.Halo2A.Structures.Textures;
using Prism.Ioc;

namespace Index.Profiles.Halo2A.Jobs
{

  public class LoadGeometryTexturesJob : JobBase
  {

    #region Properties

    private IAssetManager AssetManager { get; set; }
    private IAssetLoadContext AssetLoadContext { get; set; }

    private SceneContext Context { get; set; }
    private Dictionary<string, ITextureAsset> Textures { get; set; }

    #endregion

    #region Constructor

    public LoadGeometryTexturesJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      AssetManager = container.Resolve<IAssetManager>();
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      AssetLoadContext = Parameters.Get<IAssetLoadContext>();
      Context = Parameters.Get<SceneContext>();
      Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
    }

    protected override async Task OnExecuting()
    {
      SetStatus( "Loading Textures" );
      SetIndeterminate();
      SetIncludeUnitsInStatus();

      var toLoadSet = GatherTextures();
      await LoadTextures( toLoadSet );

      SetStatus( "Loading Additional Textures" );
      SetIndeterminate();
      SetIncludeUnitsInStatus();

      toLoadSet = GatherAdditionalTextures();
      await LoadTextures( toLoadSet );
    }

    #endregion

    #region Private Methods

    private HashSet<IAssetReference> GatherTextures()
    {
      var toLoadSet = new HashSet<IAssetReference>();
      var textureAssetReferences = AssetManager.GetAssetReferencesOfType<ITextureAsset>();

      foreach ( var material in Context.Scene.Materials )
      {
        var diffuseName = material.TextureDiffuse.FilePath;
        if ( string.IsNullOrEmpty( diffuseName ) )
          continue;

        var matches = textureAssetReferences.Where( x => x.AssetName.StartsWith( diffuseName ) );
        foreach ( var match in matches )
          toLoadSet.Add( match );
      }

      return toLoadSet;
    }

    private async Task LoadTextures( ICollection<IAssetReference> toLoadSet )
    {
      SetCompletedUnits( 0 );
      SetTotalUnits( toLoadSet.Count );
      SetIndeterminate( false );

      var jobs = new List<Task>();
      var jobSemaphore = new SemaphoreSlim( 5 );
      foreach ( var assetToLoad in toLoadSet )
      {
        jobSemaphore.Wait();
        var loadJob = AssetManager.LoadAsset<ITextureAsset>( assetToLoad, AssetLoadContext );
        loadJob.RegisterCompletionCallback( job =>
        {
          lock ( Textures )
          {
            var texture = loadJob.Result;
            Textures.Add( texture.AssetName, texture );
            IncreaseCompletedUnits( 1 );
            jobSemaphore.Release();
          }
        } );
        jobs.Add( loadJob.Completion );
      }

      await Task.WhenAll( jobs );
    }

    private HashSet<IAssetReference> GatherAdditionalTextures()
    {
      var toLoadSet = new HashSet<IAssetReference>();
      var loadedTextures = Textures.Values.ToArray();
      var textureAssetReferences = AssetManager.GetAssetReferencesOfType<ITextureAsset>();

      foreach ( var loadedTexture in loadedTextures )
      {
        var texName = loadedTexture.AssetName;
        var tdName = Path.ChangeExtension( texName, ".td" );

        if ( !loadedTexture.AdditionalData.TryGetValue( tdName, out var tdData ) )
          continue;

        var serializer = new FileScriptingSerializer<TextureDefinition>();
        var td = serializer.Deserialize( tdData );

        foreach ( var additionalTexture in td.GetTextureNames() )
        {
          if ( Textures.ContainsKey( additionalTexture ) )
            continue;

          var matches = textureAssetReferences.Where( x => x.AssetName.StartsWith( additionalTexture ) );
          foreach ( var match in matches )
            toLoadSet.Add( match );
        }
      }

      return toLoadSet;
    }

    #endregion

  }

}
