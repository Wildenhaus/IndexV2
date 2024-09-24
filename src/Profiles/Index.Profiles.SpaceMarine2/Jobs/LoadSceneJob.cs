using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Assets;
using Index.Profiles.SpaceMarine2.Meshes;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Jobs
{
  public class LoadSceneJob : CompositeJobBase<SM2SceneAsset>
  {

    #region Constructor

    public LoadSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      var assetReference = Parameters.Get<IAssetReference>();
      Name = $"Loading Scene {assetReference.AssetName}";
      SetStatus( Name );

      Parameters.Set<IAssetLoadContext>( new AssetLoadContext() );

      Parameters.Set( "LodMeshSet", new HashSet<string>() );
      Parameters.Set( "VolumeMeshSet", new HashSet<string>() );
    }

    #endregion

    #region Overrides

    protected override void CreateSubJobs()
    {
      AddJob<DeserializeSceneJob>();
      AddJob<ConvertGeometryJob>();
      AddJob<LoadGeometryTexturesJob>();
      AddJob<IdentifyMeshesJob>();
      AddJob<ConvertScenePropsJob>();
      //AddJob<ConvertSceneLightsJob>();
    }

    protected override async Task OnCompleted()
    {
      var assetReference = Parameters.Get<IAssetReference>();
      var asset = new SM2SceneAsset( assetReference );
      asset.AssetLoadContext = Parameters.Get<IAssetLoadContext>();
      asset.AssimpScene = Parameters.Get<SceneContext>().Scene;
      asset.Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
      asset.LodMeshNames = Parameters.Get<ISet<string>>( "LodMeshSet" );
      asset.VolumeMeshNames = Parameters.Get<ISet<string>>( "VolumeMeshSet" );

      SetResult( asset );
    }

    #endregion

  }

}
