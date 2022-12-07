using Assimp;

namespace Index.Profiles.HaloCEA.Meshes
{

  public class SceneContext
  {

    #region Properties

    public Scene Scene { get; }

    public Dictionary<short, Node> Nodes { get; }

    public Dictionary<short, Bone> BonesByObjectId { get; }
    public Dictionary<int, Bone> BonesByBoneIndex { get; }

    public Node RootNode
    {
      get => Scene.RootNode;
      set => Scene.RootNode = value;
    }

    #endregion

  }

}
