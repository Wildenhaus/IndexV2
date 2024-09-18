using Assimp;
using HelixToolkit.SharpDX.Core;
using LibSaber.SpaceMarine2.Structures;
using LibSaber.IO;

namespace Index.Profiles.SpaceMarine2.Meshes
{

  public class SceneContext
  {

    #region Properties

    public Scene Scene { get; }
    public string Name { get; }
    public Node RootNode => Scene.RootNode;

    public Stream Stream { get; }
    public NativeReader Reader { get; }
    public objGEOM_MNG GeometryGraph { get; }

    public Dictionary<short, Bone> Bones { get; }
    public Dictionary<short, Node> Nodes { get; }
    public Dictionary<string, Node> NodeNames { get; }
    public Dictionary<string, int> MaterialIndices { get; }
    public Dictionary<short, MeshBuilder> SkinCompounds { get; }
    public Dictionary<short, short> LodIndices { get; }

    #endregion

    #region Constructor

    public SceneContext( string name, objGEOM_MNG graph, Stream stream )
    {
      Name = name;
      Scene = new Scene();
      Scene.RootNode = new Node(name);

      Stream = stream;
      Reader = new NativeReader( stream, Endianness.LittleEndian );
      GeometryGraph = graph;

      Bones = new Dictionary<short, Bone>();
      Nodes = new Dictionary<short, Node>();
      NodeNames = new Dictionary<string, Node>();
      MaterialIndices = new Dictionary<string, int>();
      SkinCompounds = new Dictionary<short, MeshBuilder>();
      LodIndices = new Dictionary<short, short>();

      Scene.Materials.Add( new Material() { Name = "DefaultMaterial" } );
    }

    //public void AddLodDefinitions( IList<objLOD_ROOT> lodDefinitions )
    //{
    //  if ( lodDefinitions is null )
    //    return;

    //  foreach ( var lodDefinition in lodDefinitions )
    //    LodIndices.Add( lodDefinition.ObjectId, lodDefinition.Index );
    //}

    #endregion

  }

}
