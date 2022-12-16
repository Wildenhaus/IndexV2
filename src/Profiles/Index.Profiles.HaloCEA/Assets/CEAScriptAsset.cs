using Index.Domain.Assets;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEAScriptAsset : CEACacheBlockEntryAsset
  {

    public override string TypeName => "Script";

    public CEAScriptAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
