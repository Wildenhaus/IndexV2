using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEATemplateAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Template (Model)";

    #endregion

    #region Constructor

    public CEATemplateAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

}
