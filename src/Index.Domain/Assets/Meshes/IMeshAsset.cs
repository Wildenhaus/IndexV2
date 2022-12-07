using Index.Domain.Editors;

namespace Index.Domain.Assets.Meshes
{

  public interface IMeshAsset : IAsset
  {

    #region Properties

    string IAsset.TypeName => "Mesh";
    string IAsset.EditorKey => DefaultEditorKeys.MeshEditorKey;

    public Assimp.Scene AssimpScene { get; }

    #endregion

  }

}
