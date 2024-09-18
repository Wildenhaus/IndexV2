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

      Parameters.Set<IAssetLoadContext>( new AssetLoadContext() );
    }

    protected override void CreateSubJobs()
    {
      AddJob<DeserializeTemplateJob>();
      AddJob<ConvertGeometryJob>();
      AddJob<LoadGeometryTexturesJob>();
    }

    protected override async Task OnCompleted()
    {
      var assetReference = Parameters.Get<IAssetReference>();
      var asset = new SM2TemplateAsset( assetReference );
      asset.AssetLoadContext = Parameters.Get<IAssetLoadContext>();
      asset.AssimpScene = Parameters.Get<SceneContext>().Scene;
      asset.Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );

      SetResult( asset );
    }

  }

}
