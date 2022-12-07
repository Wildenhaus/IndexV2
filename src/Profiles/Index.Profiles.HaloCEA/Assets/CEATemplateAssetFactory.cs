using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.HaloCEA.Jobs;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEATemplateAssetFactory : AssetFactoryBase<MeshAsset>
  {

    #region Constructor

    public CEATemplateAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<MeshAsset> LoadAsset( IAssetReference assetReference )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );

      return JobManager.StartJob<LoadTemplateJob>( parameters );
    }

    #endregion

  }

}
