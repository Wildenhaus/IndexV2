using Assimp;
using Index.Profiles.HaloCEA.Common;
using LibSaber.HaloCEA.Structures;

namespace Index.Profiles.HaloCEA.Meshes
{

  public class SceneContext
  {

    #region Properties

    public Scene Scene { get; private set; }

    public Dictionary<short, Node> Nodes { get; private set; }

    public Dictionary<short, SaberObject> Objects { get; private set; }
    public SaberObject RootObject { get; private set; }

    public Dictionary<int, SaberObject> BoneObjects { get; private set; }
    public HashSet<short> SkinCompoundIds { get; private set; }
    public HashSet<short> SharingObjectIds { get; private set; }

    public System.Numerics.Matrix4x4[] InverseMatrices { get; set; }
    public Dictionary<short, Matrix4x4> WorldTransforms { get; private set; }

    public Node RootNode
    {
      get => Scene.RootNode;
      set => Scene.RootNode = value;
    }

    #endregion

    #region Constructor

    private SceneContext()
    {
      Scene = new Scene();
      RootNode = new Node( "ROOT" );

      Nodes = new Dictionary<short, Node>();
      WorldTransforms = new Dictionary<short, Matrix4x4>();
    }

    public static SceneContext Create( IList<SaberObject> objects )
    {
      var ctx = new SceneContext();

      ctx.InitializeObjectGraph( objects );

      return ctx;
    }

    #endregion

    #region Public Methods

    public Node AddMesh( Mesh mesh )
    {
      var meshNode = new Node( mesh.Name, RootNode );
      RootNode.Children.Add( meshNode );

      Scene.Meshes.Add( mesh );
      meshNode.MeshIndices.Add( Scene.MeshCount - 1 );

      return meshNode;
    }

    public int AddMaterial( Material material )
    {
      var matIndex = Scene.MaterialCount;
      Scene.Materials.Add( material );
      return matIndex;
    }

    public IEnumerable<SaberObject> EnumerateObjectChildren( short objectId )
      => Objects.Values.Where( x => x.ParentId == objectId );

    public IEnumerable<SaberObject> EnumerateObjectChildren( SaberObject parentObject )
      => Objects.Values.Where( x => x.ParentId == parentObject.ObjectInfo.Id );

    public bool IsSharingObject( SaberObject obj )
      => SharingObjectIds.Contains( obj.ObjectInfo.Id );

    public bool IsSkinCompoundProviderObject( SaberObject obj )
      => SkinCompoundIds.Contains( obj.ObjectInfo.Id );

    #endregion

    #region Private Methods

    private void InitializeObjectGraph( IList<SaberObject> objects )
    {
      Objects = new Dictionary<short, SaberObject>();
      BoneObjects = new Dictionary<int, SaberObject>();
      SkinCompoundIds = new HashSet<short>();
      SharingObjectIds = new HashSet<short>();

      foreach ( var obj in objects )
      {
        Objects.Add( obj.ObjectInfo.Id, obj );

        // Check if root object
        if ( !obj.ParentId.HasValue )
        {
          ASSERT( RootObject is null, "Multiple root objects detected." );
          RootObject = obj;
        }

        // Check for BoneIds
        if ( obj.BoneId != -1 )
          BoneObjects.Add( obj.BoneId, obj );

        // Check for skin compound
        if ( obj.SubmeshData != null )
        {
          foreach ( var submeshInfo in obj.SubmeshData.SubmeshList )
            if ( submeshInfo.SkinCompoundId.HasValue )
              SkinCompoundIds.Add( submeshInfo.SkinCompoundId.Value );
        }

        // Check for SharingObject
        if ( obj.SharingObjectInfo != null )
          SharingObjectIds.Add( obj.SharingObjectInfo.SharingObjectId );
      }
    }

    #endregion

  }

}
