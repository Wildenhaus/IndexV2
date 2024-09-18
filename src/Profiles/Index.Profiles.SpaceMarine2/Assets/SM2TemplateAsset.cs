using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Domain.FileSystem;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Jobs;
using LibSaber.SpaceMarine2.Structures.Resources;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Assets
{

  public class SM2TemplateAsset : MeshAsset
  {
    #region Properties

    public override string TypeName => "Model";

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

    public override IJob<SM2TemplateAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      if ( assetLoadContext is null )
        assetLoadContext = new AssetLoadContext();

      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadTemplateJob>( parameters );
    }

    #endregion

  }

}
