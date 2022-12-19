using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Jobs;
using Index.Profiles.Halo2A.Jobs;
using Index.Textures;
using Prism.Ioc;

namespace Index.Profiles.Halo2A.Assets
{

  public class H2ATextureAssetFactory : AssetFactoryBase<DxgiTextureAsset>
  {

    #region Constructor

    public H2ATextureAssetFactory( IContainerProvider containerProvider )
      : base( containerProvider )
    {
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
