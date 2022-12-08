using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEASceneAsset : MeshAsset
  {

    #region Properties

    public override string TypeName => "Scene";

    #endregion

    #region Constructor

    public CEASceneAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

}
