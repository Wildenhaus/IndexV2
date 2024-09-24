using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Assets;
using Index.Domain.Assets.Text;
using Index.Domain.Jobs;
using Index.Jobs;
using Prism.Ioc;

namespace Index.Profiles.Halo2A.Assets
{

  public abstract class H2ATextAsset : TextAsset
  {

    protected H2ATextAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

  public class H2ATextAssetFactory<TAsset> : AssetFactoryBase<TAsset>
    where TAsset : H2ATextAsset
  {

    protected H2ATextAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    public override IJob<TAsset> LoadAsset( IAssetReference assetReference, IAssetLoadContext assetLoadContext = null )
    {
      var asset = CreateAsset( assetReference );
      asset.TextStream = assetReference.Node.Open();

      return CompletedJob.FromResult( asset );
    }

    private TAsset CreateAsset( IAssetReference assetReference )
      => ( TAsset ) Activator.CreateInstance( typeof( TAsset ), new object[] { assetReference } );

  }

}
