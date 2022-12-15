using Assimp;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;
using M4 = System.Numerics.Matrix4x4;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class ConvertGeometryJob : JobBase
  {

    #region Properties

    protected SceneContext Context { get; set; }
    protected Dictionary<string, ITextureAsset> Textures { get; set; }

    #endregion

    #region Constructor

    public ConvertGeometryJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      SetIncludeUnitsInStatus();

      Context = Parameters.Get<SceneContext>();
      Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
    }

    protected override async Task OnExecuting()
    {
      var textureList = Parameters.Get<TextureList>();
      AddMaterials( textureList );
      AddNodes();
      AddMeshes();
    }

    #endregion

    #region Private Methods

    protected void AddNodes()
    {
      SetStatus( "Initializing Nodes" );
      SetCompletedUnits( 0 );
      SetTotalUnits( Context.Objects.Count );
      SetIndeterminate( false );

      AddNode( Context.RootObject );
    }

    protected void AddNode( SaberObject obj, Node parent = null )
    {
      if ( parent is null )
        parent = Context.Scene.RootNode;

      var node = new Node( obj.ObjectInfo.Name, parent );
      parent.Children.Add( node );
      Context.Nodes[ obj.ObjectInfo.Id ] = node;

      var transform = CalculateWorldTransform( obj );
      node.Transform = transform;

      foreach ( var child in Context.EnumerateObjectChildren( obj ) )
        AddNode( child, node );

      IncreaseCompletedUnits( 1 );
    }

    protected void AddMeshes()
    {
      SetStatus( "Preparing Meshes" );
      SetCompletedUnits( 0 );
      SetTotalUnits( Context.Objects.Count );
      SetIndeterminate( false );

      foreach ( var obj in Context.Objects.Values )
      {
        IncreaseCompletedUnits( 1 );

        // Filter out non-mesh objects
        if ( Context.IsSkinCompoundProviderObject( obj ) )
          continue;
        if ( Context.IsSharingObject( obj ) )
          continue;
        if ( obj.ObjectInfo.VertexCount == 0 && obj.SubmeshData is null )
          continue;

        AddMesh( obj );
      }
    }

    private void AddMesh( SaberObject obj )
    {
      if ( obj.ObjectInfo.Name == "shield" )
        return;

      if ( obj.SubmeshData.SubmeshList.Count == 0 )
        System.Diagnostics.Debugger.Break();

      foreach ( var submeshInfo in obj.SubmeshData.SubmeshList )
        AddSubMesh( obj, submeshInfo );
    }

    private void AddSubMesh( SaberObject obj, SubmeshInfo submeshInfo )
    {
      var meshBuilder = MeshBuilder.Build( Context, obj, submeshInfo );

      var meshNodeParent = Context.Nodes[ obj.ObjectInfo.Id ];
      var meshNode = new Node( obj.ObjectInfo.Name, meshNodeParent );
      meshNodeParent.Children.Add( meshNode );

      var meshIndex = Context.Scene.MeshCount;
      Context.Scene.Meshes.Add( meshBuilder.Mesh );
      meshNode.MeshIndices.Add( meshIndex );

      if ( meshBuilder.SkinCompoundObject is not null )
      {
        var worldTransform = Context.WorldTransforms[ obj.ObjectInfo.Id ];
        var localTransform = obj.Matrix.Value.ToAssimp();
        localTransform.Transpose();

        meshNodeParent.Transform = localTransform * worldTransform;
      }
    }

    private void AddMaterials( IList<TextureListEntry> textures )
    {
      foreach ( var baseTextureName in textures )
        AddMaterial( baseTextureName );

      if ( Context.Scene.MaterialCount == 0 )
        Context.Scene.Materials.Add( new Material() { Name = "DefaultMaterial" } );
    }

    private void AddMaterial( string baseTextureName )
    {
      var material = MaterialBuilder.Build( Context, baseTextureName, Textures );
      Context.Scene.Materials.Add( material );
      IncreaseCompletedUnits( 1 );
    }

    protected Matrix4x4 CalculateWorldTransform( SaberObject obj )
    {
      // I have no idea why the f#!% this works
      // I literally just tried a bunch of combinations.
      var transform = M4.Transpose( obj.Matrix.Value );

      var objectId = obj.ObjectInfo.Id;
      var inverseMatrices = Context.InverseMatrices;
      var worldTransforms = Context.WorldTransforms;
      if ( inverseMatrices is not null )
      {
        if ( objectId < inverseMatrices.Length )
        {
          var parentId = obj.ParentId;
          var parentTransform = M4.Identity;
          if ( parentId.HasValue )
            parentTransform = worldTransforms[ ( short ) parentId.Value ].ToNumerics();

          var invMatrix = M4.Transpose( inverseMatrices[ objectId ] );
          M4.Invert( invMatrix, out invMatrix );

          M4.Invert( parentTransform, out var invParent );
          transform = invParent * invMatrix;
        }
      }

      // Compute the world transform from the inverse matrix
      {
        var parentId = obj.ParentId;
        var parentTransform = M4.Identity;
        if ( parentId.HasValue )
          parentTransform = worldTransforms[ ( short ) parentId.Value ].ToNumerics();

        var nodeTransform = transform;
        var worldTransform = parentTransform * nodeTransform;
        worldTransforms.Add( objectId, worldTransform.ToAssimp() );
      }

      return transform.ToAssimp();
    }

    #endregion

  }

}
