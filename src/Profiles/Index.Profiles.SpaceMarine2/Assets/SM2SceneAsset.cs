using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Jobs;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Assets
{

  public class SM2SceneAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Map (Work in Progress)";

    #endregion

    #region Constructor

    public SM2SceneAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

  public class SM2SceneAssetFactory : AssetFactoryBase<SM2SceneAsset>
  {

    #region Constructor

    public SM2SceneAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<SM2SceneAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadSceneJob>( parameters );
    }

    #endregion

  }

}
