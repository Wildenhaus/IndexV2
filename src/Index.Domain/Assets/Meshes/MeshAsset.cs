using Assimp;
using Index.Domain.Editors;

namespace Index.Domain.Assets.Meshes
{

  public class MeshAsset : AssetBase, IMeshAsset
  {

    #region Properties

    public override string TypeName => "Mesh";
    public override string EditorKey => DefaultEditorKeys.MeshEditorKey;

    public Scene AssimpScene { get; set; }

    #endregion

    #region Constructor

    public MeshAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

  }

}
