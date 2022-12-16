using Index.Domain.Assets;
using Index.Domain.Assets.Text;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEAShaderCodeAsset : TextAsset
  {

    public override string TypeName => "Shader Code";

    public CEAShaderCodeAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
