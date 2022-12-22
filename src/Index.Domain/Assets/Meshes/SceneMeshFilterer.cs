using Assimp;

namespace Index.Domain.Assets.Meshes
{

  internal class SceneMeshFilterer
  {

    #region Data Members

    private readonly IMeshAsset _meshAsset;
    private readonly ISet<string> _removeSet;

    #endregion

    #region Properties

    private Scene OldScene { get; }
    private Scene NewScene { get; }

    private Dictionary<int, int> MaterialLookup { get; }
    private Dictionary<int, int> MeshLookup { get; }

    #endregion

    #region Constructor

    public SceneMeshFilterer( IMeshAsset meshAsset, ISet<string> removeSet )
    {
      _meshAsset = meshAsset;
      _removeSet = removeSet ?? new HashSet<string>();

      OldScene = meshAsset.AssimpScene;
      NewScene = new Scene();

      MaterialLookup = new Dictionary<int, int>();
      MeshLookup = new Dictionary<int, int>();
    }

    #endregion

    #region Public Methods

    public Scene RecreateScene()
    {
      if ( _removeSet.Count == 0 )
        return OldScene;

      var oldRoot = OldScene.RootNode;
      var newRoot = AddNode( oldRoot );

      NewScene.RootNode = newRoot;
      return NewScene;
    }

    #endregion

    #region Private Methods

    private int AddMaterial( Material material )
    {
      NewScene.Materials.Add( material );
      return NewScene.Materials.Count - 1;
    }

    private int AddMesh( Mesh oldMesh )
    {
      var newMesh = new Mesh( oldMesh.Name, oldMesh.PrimitiveType );
      newMesh.BiTangents.AddRange( oldMesh.BiTangents );
      newMesh.Bones.AddRange( oldMesh.Bones );
      newMesh.BoundingBox = oldMesh.BoundingBox;
      newMesh.Faces.AddRange( oldMesh.Faces );
      newMesh.MeshAnimationAttachments.AddRange( oldMesh.MeshAnimationAttachments );
      newMesh.MorphMethod = oldMesh.MorphMethod;
      newMesh.Normals.AddRange( oldMesh.Normals );
      newMesh.Tangents.AddRange( oldMesh.Tangents );
      newMesh.Vertices.AddRange( oldMesh.Vertices );

      for ( var i = 0; i < oldMesh.TextureCoordinateChannels.Length; i++ )
        newMesh.TextureCoordinateChannels[ i ].AddRange( oldMesh.TextureCoordinateChannels[ i ] );

      for ( var i = 0; i < oldMesh.UVComponentCount.Length; i++ )
        newMesh.UVComponentCount[ i ] = oldMesh.UVComponentCount[ i ];

      for ( var i = 0; i < oldMesh.VertexColorChannels.Length; i++ )
        newMesh.VertexColorChannels[ i ].AddRange( oldMesh.VertexColorChannels[ i ] );

      if ( !MaterialLookup.TryGetValue( oldMesh.MaterialIndex, out var newMaterialIndex ) )
      {
        var material = OldScene.Materials[ oldMesh.MaterialIndex ];
        newMaterialIndex = AddMaterial( material );

        MaterialLookup.Add( oldMesh.MaterialIndex, newMaterialIndex );
      }
      newMesh.MaterialIndex = newMaterialIndex;

      NewScene.Meshes.Add( newMesh );
      return NewScene.Meshes.Count - 1;
    }

    private void CopyNodeMeshes( Node oldNode, Node newNode )
    {
      foreach ( var oldMeshIndex in oldNode.MeshIndices )
      {
        if ( !MeshLookup.TryGetValue( oldMeshIndex, out var newMeshIndex ) )
        {
          var mesh = OldScene.Meshes[ oldMeshIndex ];
          newMeshIndex = AddMesh( mesh );

          MeshLookup.Add( oldMeshIndex, newMeshIndex );
        }

        newNode.MeshIndices.Add( newMeshIndex );
      }
    }

    private Node AddNode( Node oldNode, Node newParentNode = null )
    {
      if ( _removeSet.Contains( oldNode.Name ) )
        return null;

      var newNode = new Node( oldNode.Name, newParentNode );
      if ( newParentNode != null )
        newParentNode.Children.Add( newNode );

      newNode.Transform = oldNode.Transform;

      CopyNodeMeshes( oldNode, newNode );

      foreach ( var child in oldNode.Children )
        AddNode( child, newNode );

      return newNode;
    }

    #endregion

  }

}
