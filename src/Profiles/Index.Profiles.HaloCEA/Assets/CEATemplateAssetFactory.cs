using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.HaloCEA.Jobs;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEATemplateAssetFactory : AssetFactoryBase<CEATemplateAsset>
  {

    #region Constructor

    public CEATemplateAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<CEATemplateAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadTemplateJob>( parameters );
    }

    #endregion

  }

}
