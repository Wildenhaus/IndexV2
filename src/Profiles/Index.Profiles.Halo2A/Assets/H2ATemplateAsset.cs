using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.Halo2A.Jobs;
using Prism.Ioc;

namespace Index.Profiles.Halo2A.Assets
{

  public class H2ATemplateAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Template (Model)";

    #endregion

    #region Constructor

    public H2ATemplateAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

  public class H2ATemplateAssetFactory : AssetFactoryBase<H2ATemplateAsset>
  {

    #region Constructor

    public H2ATemplateAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    #endregion

    #region Overrides

    public override IJob<H2ATemplateAsset> LoadAsset( IAssetReference assetReference )
    {
      var parameters = new ParameterCollection();
      parameters.Set( assetReference );

      return JobManager.StartJob<LoadTemplateJob>( parameters );
    }

    #endregion

  }

}
