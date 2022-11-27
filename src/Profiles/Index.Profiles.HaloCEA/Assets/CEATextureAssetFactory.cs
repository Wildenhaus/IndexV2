using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Jobs;
using Index.Profiles.HaloCEA.Jobs;
using Index.Textures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEATextureAssetFactory : AssetFactoryBase<DxgiTextureAsset>
  {

    #region Data Members

    private readonly IDxgiTextureService _dxgiTextureService;

    #endregion

    #region Constructor

    public CEATextureAssetFactory( IContainerProvider containerProvider )
      : base( containerProvider )
    {
      _dxgiTextureService = containerProvider.Resolve<IDxgiTextureService>();
    }

    #endregion

    #region Overrides

    public override IJob<DxgiTextureAsset> LoadAsset( IAssetReference assetReference )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );

      return JobManager.StartJob<LoadTextureJob>( parameters );
    }

    #endregion

  }

}
