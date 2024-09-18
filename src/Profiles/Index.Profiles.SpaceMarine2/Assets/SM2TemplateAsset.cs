using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Jobs;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Assets
{

  public class SM2TemplateAsset : MeshAsset
  {
    #region Properties

    public override string TypeName => "Template (Model)";

    #endregion

    #region Constructor

    public SM2TemplateAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion
  }

  public class SM2TemplateAssetFactory : AssetFactoryBase<SM2TemplateAsset>
  {

    #region Constructor

    public SM2TemplateAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<SM2TemplateAsset> LoadAsset( IAssetReference assetReference )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );

      return JobManager.StartJob<LoadTemplateJob>( parameters );
    }

    #endregion

  }

}
