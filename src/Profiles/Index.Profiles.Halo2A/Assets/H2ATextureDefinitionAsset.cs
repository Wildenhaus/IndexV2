using Index.Domain.Assets;

namespace Index.Profiles.Halo2A.Assets
{

  public class H2ATextureDefinitionAsset : H2ATextAsset
  {

    public override string TypeName => "Texture Definition";

    public H2ATextureDefinitionAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
