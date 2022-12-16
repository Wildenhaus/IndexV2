using Index.Domain.Assets;
using Index.Domain.Jobs;
using Index.Jobs;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEAShaderCodeAssetFactory : AssetFactoryBase<CEAShaderCodeAsset>
  {

    public CEAShaderCodeAssetFactory( IContainerProvider container )
      : base( container )
    {
    }

    public override IJob<CEAShaderCodeAsset> LoadAsset( IAssetReference assetReference )
    {
      var asset = new CEAShaderCodeAsset( assetReference );
      asset.TextStream = assetReference.Node.Open();

      return CompletedJob.FromResult( asset );
    }

  }

}
