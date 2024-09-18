using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.Halo2A.Jobs;
using Prism.Ioc;

namespace Index.Profiles.Halo2A.Assets
{

  public class H2ASceneAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Scene";

    #endregion

    #region Constructor

    public H2ASceneAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

  public class H2ASceneAssetFactory : AssetFactoryBase<H2ASceneAsset>
  {

    #region Constructor

    public H2ASceneAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<H2ASceneAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadSceneJob>( parameters );
    }

    #endregion

  }

}
