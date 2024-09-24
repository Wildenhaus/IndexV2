using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Jobs;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Assets
{

  public class SM2TextureAssetFactory : AssetFactoryBase<DxgiTextureAsset>
  {
    #region Constructor

    public SM2TextureAssetFactory( IContainerProvider containerProvider )
      : base( containerProvider )
    {
    }

    #endregion

    #region Overrides

    public override IJob<DxgiTextureAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadTextureJob>( parameters );
    }

    #endregion
  }

}
