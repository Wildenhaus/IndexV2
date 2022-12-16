using Index.Domain.Assets;
using Index.Domain.Assets.Text;

namespace Index.Profiles.HaloCEA.Assets
{

  public abstract class CEACacheBlockEntryAsset : TextAsset
  {

    public CEACacheBlockEntryAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

  }

}
