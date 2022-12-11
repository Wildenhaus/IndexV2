using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Assimp;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class ConvertGeometryJob : JobBase
  {

    protected SceneContext Context { get; set; }
    protected Dictionary<string, ITextureAsset> Textures { get; set; }

    public ConvertGeometryJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    protected override async Task OnInitializing()
    {
      Context = Parameters.Get<SceneContext>();
      Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
    }

    protected override async Task OnExecuting()
    {
      var textureList = Parameters.Get<TextureList>();
      AddMaterials( textureList );
      AddNodes();
      AddMeshes();

      //foreach ( var node in Context.RootNode.EnumerateChildren() )
      //{
      //  var scale = 100f;
      //  node.Transform.Decompose( out _, out var rot, out var pos );
      //  var transform = System.Numerics.Matrix4x4.CreateFromQuaternion( new System.Numerics.Quaternion( rot.X, rot.Y, rot.Z, rot.W ) );
      //  transform *= System.Numerics.Matrix4x4.CreateScale( scale );
      //  node.Transform = transform.ToAssimp();
      //}

      //if ( Context.BoneObjects.Count > 0 )

      //{
      //  var rootBone = Context.BoneObjects[ 0 ];
      //  var armatureNode = Context.Nodes[ rootBone.ObjectInfo.Id ];
      //  var scale = 0.0328f;
      //  armatureNode.Transform.Decompose( out _, out var rot, out var pos );
      //  var transform = System.Numerics.Matrix4x4.CreateFromQuaternion( new System.Numerics.Quaternion( rot.X, rot.Y, rot.Z, rot.W ) );
      //  transform *= System.Numerics.Matrix4x4.CreateScale( scale );
      //  armatureNode.Transform = transform.ToAssimp();
      //}
    }

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

      var transform = GetTransform( obj );
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
      var mesh = MeshBuilder.Build( Context, obj, submeshInfo );

      var meshNodeParent = Context.Nodes[ obj.ObjectInfo.Id ];
      var meshNode = new Node( obj.ObjectInfo.Name, meshNodeParent );
      meshNodeParent.Children.Add( meshNode );

      var meshIndex = Context.Scene.MeshCount;
      Context.Scene.Meshes.Add( mesh );
      meshNode.MeshIndices.Add( meshIndex );

      var t = meshNodeParent.Transform;
      t.Inverse();
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

    protected Assimp.Matrix4x4 GetTransform( SaberObject obj, SubmeshInfo submeshInfo = null )
    {
      var transform = System.Numerics.Matrix4x4.Identity;

      var parent = obj;
      transform *= System.Numerics.Matrix4x4.Transpose( parent.Matrix.Value );

      var m = transform.ToAssimp();
      return m;
    }

  }

}
