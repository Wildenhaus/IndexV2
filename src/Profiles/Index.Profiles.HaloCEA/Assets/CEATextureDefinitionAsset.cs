using Index.Domain.Assets;
using Index.Domain.Assets.Text;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEATextureDefinitionAsset : CEACacheBlockEntryAsset
  {

    public override string TypeName => "Texture Definition";

    public CEATextureDefinitionAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
