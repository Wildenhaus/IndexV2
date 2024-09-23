using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.Jobs;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Meshes;
using LibSaber.SpaceMarine2.Serialization.Scripting;
using LibSaber.SpaceMarine2.Structures.Textures;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Jobs
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

        var matches = textureAssetReferences.Where( x => Path.GetFileName(x.AssetName).Contains( diffuseName ) );
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
      foreach ( var assetToLoad in toLoadSet )
      {
        lock ( Textures )
        {
          if ( Textures.ContainsKey( assetToLoad.AssetName ) )
            continue;
        }

        var loadJob = AssetManager.LoadAsset<ITextureAsset>( assetToLoad, AssetLoadContext );
        loadJob.RegisterCompletionCallback( job =>
        {
          lock ( Textures )
          {
            var texture = loadJob.Result;
            var texName = Path.GetFileNameWithoutExtension( texture.AssetName.Replace(".resource", "") );
            Textures.Add(texName, texture );
            IncreaseCompletedUnits( 1 );
          }
        } );
        jobs.Add( loadJob.Completion );
      }

      await Task.WhenAll( jobs );
    }

    private HashSet<IAssetReference> GatherAdditionalTextures()
    {
      var toLoadSet = new HashSet<IAssetReference>();
      //var loadedTextures = Textures.Values.ToArray();
      //var textureAssetReferences = AssetManager.GetAssetReferencesOfType<ITextureAsset>();

      //foreach ( var loadedTexture in loadedTextures )
      //{
      //  var texName = loadedTexture.AssetName;
      //  var tdName = Path.ChangeExtension( texName, ".td" );

      //  if ( !loadedTexture.AdditionalData.TryGetValue( tdName, out var tdData ) )
      //    continue;

      //  var serializer = new FileScriptingSerializer<TextureDefinition>();
      //  var td = serializer.Deserialize( tdData );

      //  foreach ( var additionalTexture in td.GetTextureNames() )
      //  {
      //    if ( string.IsNullOrWhiteSpace( additionalTexture ) )
      //      continue;

      //    if ( Textures.ContainsKey( additionalTexture ) )
      //      continue;

      //    var matches = textureAssetReferences.Where( x => x.AssetName.StartsWith( additionalTexture ) );
      //    foreach ( var match in matches )
      //      toLoadSet.Add( match );
      //  }
      //}

      return toLoadSet;
    }

    #endregion

  }

}
