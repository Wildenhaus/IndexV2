using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.HaloCEA.Assets;
using Index.Profiles.HaloCEA.Meshes;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class LoadSceneJob : CompositeJobBase<CEASceneAsset>
  {

    #region Constructor

    public LoadSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      var assetReference = Parameters.Get<IAssetReference>();
      Name = $"Loading Scene {assetReference.AssetName}";
      SetStatus( Name );

      Parameters.Set<IAssetLoadContext>( new AssetLoadContext() );
    }

    #endregion

    protected override void CreateSubJobs()
    {
      AddJob<DeserializeSceneJob>();
      AddJob<LoadGeometryTexturesJob>();
      AddJob<ConvertGeometryJob>();
      AddJob<ConvertScenePropsJob>();
      AddJob<ConvertSceneLightsJob>();
      AddJob<IdentifyMeshesJob>();
    }

    protected override async Task OnCompleted()
    {
      var assetReference = Parameters.Get<IAssetReference>();
      var asset = new CEASceneAsset( assetReference );
      asset.AssimpScene = Parameters.Get<SceneContext>().Scene;
      asset.Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
      asset.LodMeshNames = Parameters.Get<ISet<string>>( "LodMeshSet" );
      asset.VolumeMeshNames = Parameters.Get<ISet<string>>( "VolumeMeshSet" );

      SetResult( asset );
    }

  }

}
