using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.HaloCEA.Assets;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class LoadEmbeddedTemplateJob : CompositeJobBase<CEATemplateAsset>
  {

    #region Constructor

    public LoadEmbeddedTemplateJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      var templateData = Parameters.Get<Data_02E4>();
      Name = $"Loading Embedded Template {templateData}";
      SetStatus( Name );
    }

    #endregion

    #region Overrides

    protected override void CreateSubJobs()
    {
      AddJob<ConvertGeometryJob>();
      AddJob<IdentifyMeshesJob>();
    }

    protected override async Task OnCompleted()
    {
      var asset = new CEATemplateAsset( null );
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
