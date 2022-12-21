using Index.Domain.Assets;

namespace Index.Profiles.Halo2A.Assets
{

  public class H2AShaderCodeAsset : H2ATextAsset
  {

    public override string TypeName => "Shader Code";

    public H2AShaderCodeAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
