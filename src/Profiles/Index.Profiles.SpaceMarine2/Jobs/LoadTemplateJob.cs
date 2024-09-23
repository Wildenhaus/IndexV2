using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Assets;
using Index.Profiles.SpaceMarine2.Meshes;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Jobs
{
  public class LoadTemplateJob : CompositeJobBase<SM2TemplateAsset>
  {

    public LoadTemplateJob( IContainerProvider container, IParameterCollection parameters ) 
      : base( container, parameters )
    {
      var assetReference = Parameters.Get<IAssetReference>();
      Name = $"Loading Template {assetReference.AssetName}";
      SetStatus( Name );

      if(!Parameters.TryGet<IAssetLoadContext>( out _ ))
        Parameters.Set<IAssetLoadContext>( new AssetLoadContext() );

      Parameters.Set( "LodMeshSet", new HashSet<string>() );
      Parameters.Set( "VolumeMeshSet", new HashSet<string>() );
    }

    protected override void CreateSubJobs()
    {
      AddJob<DeserializeTemplateJob>();
      AddJob<ConvertGeometryJob>();
      AddJob<LoadGeometryTexturesJob>();
      AddJob<IdentifyMeshesJob>();
    }

    protected override async Task OnCompleted()
    {
      var assetReference = Parameters.Get<IAssetReference>();
      var asset = new SM2TemplateAsset( assetReference );
      asset.AssetLoadContext = Parameters.Get<IAssetLoadContext>();
      asset.AssimpScene = Parameters.Get<SceneContext>().Scene;
      asset.Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
      asset.LodMeshNames = Parameters.Get<HashSet<string>>( "LodMeshSet" );
      asset.VolumeMeshNames = Parameters.Get<HashSet<string>>( "VolumeMeshSet" );

      SetResult( asset );
    }

  }

}
