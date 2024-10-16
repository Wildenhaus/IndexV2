﻿using Index.Domain.Assets;
using Index.Jobs;
using Index.Profiles.HaloCEA.Jobs;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEASceneAssetFactory : AssetFactoryBase<CEASceneAsset>
  {

    #region Constructor

    public CEASceneAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<CEASceneAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );
      parameters.Set( assetLoadContext );

      return JobManager.StartJob<LoadSceneJob>( parameters );
    }

    #endregion

  }

}
