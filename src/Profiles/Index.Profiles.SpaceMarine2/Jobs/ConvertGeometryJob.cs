using Assimp;
using Index.Domain.Assets.Textures;
using Index.Domain.FileSystem;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Common;
using Index.Profiles.SpaceMarine2.Meshes;
using LibSaber.SpaceMarine2.Structures;
using Prism.Ioc;

namespace Index.Profiles.SpaceMarine2.Jobs
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
      ConvertObjects();
      //FixupArmature();
    }

    #endregion

    #region Private Methods

    private void ConvertObjects()
    {
      AddNodes( Context.GeometryGraph.objects );
      //BuildSkinCompounds();
      AddMeshNodes( Context.GeometryGraph.objects );
      AddRemainingMeshBones();
      //RenameBones();

      //using ( var ctx = new AssimpContext() )
      //{
      //  var s = ctx.ExportFile( Context.Scene, @"E:\test\test.fbx", "fbx" );

      //  var test = ctx.ImportFile( @"E:\test\test.fbx" );
      //}
    }

    private void BuildSkinCompounds()
    {
      SetStatus( "Building Skin Compounds" );
      SetIndeterminate();

      var skinCompoundIds = Context.GeometryGraph.SubMeshes
        .Select( x => x.BufferInfo.SkinCompoundId )
        .Where( x => x >= 0 )
        .Distinct()
        .ToArray();

      SetTotalUnits( skinCompoundIds.Length );
      SetCompletedUnits( 0 );
      SetUnitName( "Skin Compounds Built" );

      foreach ( var skinCompoundId in skinCompoundIds )
      {
        var skinCompoundObject = Context.GeometryGraph.objects[ skinCompoundId ];
        if ( !skinCompoundObject.SubMeshes.Any() )
          continue;

        var builder = new MeshBuilder( Context, skinCompoundObject, skinCompoundObject.SubMeshes.First() );
        builder.Build();

        Context.SkinCompounds[ skinCompoundId ] = builder;
        IncreaseCompletedUnits( 1 );
      }
    }

    private void AddNodes( List<objOBJ> objects )
    {
      SetStatus( "Initializing Nodes" );
      SetUnitName( "Nodes Initialized" );
      SetTotalUnits( objects.Count );
      SetCompletedUnits( 0 );

      var rootNode = Context.RootNode;
      var rootObject = Context.GeometryGraph.RootObject;
      if ( rootObject.ReadName is null )
        rootObject.ReadName = Context.Name;

      AddNodesRecursive(rootObject, rootNode);

      //foreach ( var obj in objects )
      //{
      //  var path = obj.UnkName;
      //  if ( string.IsNullOrEmpty( path ) )
      //    continue;

      //  var pathParts = path.Split( '|', StringSplitOptions.RemoveEmptyEntries );
      //  var parentNode = rootNode;
      //  foreach ( var part in pathParts )
      //  {
      //    if ( !Context.NodeNames.TryGetValue( part, out var newNode ) )
      //    {
      //      newNode = new Node( part, parentNode );
      //      parentNode.Children.Add( newNode );
      //      Context.NodeNames.Add( part, newNode );
      //    }

      //    parentNode = newNode;
      //  }

      //  var nodeName = pathParts.Last();
      //  var node = Context.NodeNames[ nodeName ];
      //  Context.Nodes.Add( obj.id, node );

      //  var transform = obj.MatrixModel.ToAssimp();
      //  transform.Transpose();
      //  node.Transform = transform;

      //  IncreaseCompletedUnits( 1 );
      //}
    }

    private void AddNodesRecursive(objOBJ obj, Node parentNode)
    {
      if ( obj.SubMeshes.Any() )
        return;

      var objName = obj.GetName();

      var node = new Node( objName, parentNode );
      parentNode.Children.Add( node );
      Context.Nodes.Add( obj.id, node );
      Context.NodeNames[ objName ] = node;

      var transform = obj.MatrixModel.ToAssimp();
      transform.Transpose();
      node.Transform = transform;

      IncreaseCompletedUnits( 1 );

      foreach ( var childObj in obj.EnumerateChildren() )
        AddNodesRecursive( childObj, node );
    }

    private void AddMeshNodes( List<objOBJ> objects )
    {
      SetStatus( "Building Meshes" );
      SetIndeterminate();
      SetUnitName( "Meshes Built" );

      var objectsWithMeshes = objects.Where( x => x.SubMeshes.Any() ).ToArray();
      var submeshCount = objectsWithMeshes.Sum( x => x.SubMeshes.Count() );

      SetCompletedUnits( 0 );
      SetTotalUnits( submeshCount );

      foreach ( var obj in objectsWithMeshes )
      {
        if ( Context.SkinCompounds.ContainsKey( obj.id ) )
          continue;

        AddSubMeshes( obj );
      }
    }

    private void AddSubMeshes( objOBJ obj )
    {
      foreach ( var submesh in obj.SubMeshes )
      {
        var node = new Node( obj.GetName(), Context.RootNode );
        Context.RootNode.Children.Add( node );

        var builder = new MeshBuilder( Context, obj, submesh );
        var mesh = builder.Build();

        Context.Scene.Meshes.Add( mesh );
        var meshId = Context.Scene.Meshes.Count - 1;
        node.MeshIndices.Add( meshId );

        var transform = obj.MatrixLT.ToAssimp();
        transform.Transpose();
        node.Transform = transform;

        var meshName = obj.GetName();
        if ( !mesh.HasBones && obj.Parent != null )
          builder.ParentMeshToBone( obj.Parent );

        IncreaseCompletedUnits( 1 );
      }
    }

    private void AddRemainingMeshBones()
    {
      // Blender sometimes freaks out if bones in the hierarchy chain aren't on the meshes.
      // Hence this icky looking method.

      var boneLookup = new Dictionary<string, Bone>();
      foreach ( var mesh in Context.Scene.Meshes )
      {
        foreach ( var bone in mesh.Bones )
          if ( !boneLookup.ContainsKey( bone.Name ) )
            boneLookup.Add( bone.Name, new Bone { Name = bone.Name, OffsetMatrix = bone.OffsetMatrix } );
      }

      foreach ( var mesh in Context.Scene.Meshes )
      {
        foreach ( var meshBone in mesh.Bones.ToList() )
        {
          var meshBoneNode = Context.Scene.RootNode.FindNode( meshBone.Name );
          if ( meshBoneNode is null )
            continue;

          var parent = meshBoneNode.Parent;
          while ( parent != null && !parent.HasMeshes )
          {
            if ( !mesh.Bones.Any( x => x.Name == parent.Name ) )
              if ( boneLookup.TryGetValue( parent.Name, out var parentBone ) )
                mesh.Bones.Add( new Bone { Name = parentBone.Name, OffsetMatrix = parentBone.OffsetMatrix } );

            parent = parent.Parent;
          }
        }
      }

      // Issue #20: Add zero weights to any unused bones for animation retargeting
      //var boneObjects = Context.GeometryGraph.objects
      //  .Where( x => x.GetName() == x.GetName() )
      //  .ToArray();

      //foreach ( var boneObject in boneObjects )
      //{
      //  var boneObjectName = boneObject.GetName();
      //  if ( string.IsNullOrEmpty( boneObjectName ) )
      //    continue;

      //  if ( boneLookup.ContainsKey( boneObject.GetName() ) )
      //    continue;

      //  var boneParent = boneObject.Parent;
      //  if ( boneParent is null )
      //    continue;

      //  foreach ( var mesh in Context.Scene.Meshes )
      //  {
      //    if ( !mesh.Bones.Any( x => x.Name == boneParent.GetName() ) )
      //      continue;

      //    System.Numerics.Matrix4x4.Invert( boneObject.MatrixLT, out var invMatrix );
      //    var transform = invMatrix.ToAssimp();
      //    transform.Transpose();

      //    var bone = new Bone
      //    {
      //      Name = boneObject.GetName(),
      //      OffsetMatrix = transform
      //    };

      //    for ( var i = 0; i < mesh.VertexCount; i++ )
      //      bone.VertexWeights.Add( new VertexWeight( i, 0f ) );

      //    mesh.Bones.Add( bone );
      //  }
      //}
    }

    private void RenameBones()
    {
      // Issue #15 - Remove prefix from bones to support 
      // base Halo 2 animation retargeting.

      // Rename Nodes
      var nodeQueue = new Queue<Node>();
      nodeQueue.Enqueue( Context.Scene.RootNode );
      while ( nodeQueue.TryDequeue( out var node ) )
      {
        if ( !string.IsNullOrEmpty( node.Name ) )
          if ( node.Name.StartsWith( "_b_" ) )
            node.Name = node.Name.Substring( 3 );

        foreach ( var child in node.Children )
          nodeQueue.Enqueue( child );
      }

      // Rename mesh bones
      foreach ( var mesh in Context.Scene.Meshes )
      {
        foreach ( var bone in mesh.Bones )
        {
          if ( bone.Name.StartsWith( "_b_" ) )
            bone.Name = bone.Name.Substring( 3 );
        }
      }
    }

    private void FixupArmature()
    {
      var armatureNode = Context.Scene.RootNode.FindNode( Context.Name );
      if ( armatureNode is null )
        return;

      // Fix Scale
      const float METER_TO_FEET = 3.2808399f;
      void SetScale( Node node, float scale = METER_TO_FEET * 10 )
      {
        var transform = node.Transform;
        transform.A1 *= scale;
        transform.A2 *= scale;
        transform.A3 *= scale;
        transform.A4 *= scale;
        transform.B1 *= scale;
        transform.B2 *= scale;
        transform.B3 *= scale;
        transform.B4 *= scale;
        transform.C1 *= scale;
        transform.C2 *= scale;
        transform.C3 *= scale;
        transform.C4 *= scale;
        node.Transform = transform;
      }

      //foreach ( var node in Context.Scene.RootNode.Children )
        SetScale( armatureNode );
    }

    #endregion

  }

}