using Assimp;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public abstract class LoadGeometryJobBase<TAsset> : JobBase<TAsset>
    where TAsset : class, IMeshAsset
  {

    #region Properties

    protected SceneContext Context { get; private set; }

    protected IAssetReference AssetReference { get; }

    #endregion

    #region Constructor

    protected LoadGeometryJobBase( IContainerProvider container, IParameterCollection parameters = null )
      : base( container, parameters )
    {
      AssetReference = parameters.Get<IAssetReference>();
      Name = $"Loading {AssetReference.AssetName}";
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      SetStatus( "Deserializing Data" );
      SetIndeterminate();

      Context = await CreateSceneContext( AssetReference );
    }

    protected override async Task OnExecuting()
    {
      CreateScene( Context );

      var asset = CreateAsset( Context.Scene );
      SetResult( asset );
    }

    #endregion

    #region Abstract Methods

    protected abstract Task<SceneContext> CreateSceneContext( IAssetReference assetReference );

    protected abstract IList<TextureListEntry> GetTextureList();

    protected abstract TAsset CreateAsset( Scene assimpScene );

    #endregion

    #region Virtual Methods

    protected virtual void CreateScene( SceneContext context )
    {
      AddNodes();
      AddMeshes();
      AddMaterials( GetTextureList() );
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

      // Ensure armature is at root
      if ( obj.BoneId == 0 )
        parent = Context.Scene.RootNode;

      var node = new Node( obj.ObjectInfo.Name, parent );
      parent.Children.Add( node );

      var transform = obj.Matrix.Value.ToMatrix4();
      transform.Transpose();
      node.Transform = transform;

      foreach ( var child in Context.EnumerateObjectChildren( obj ) )
        AddNode( child, node );

      IncreaseCompletedUnits( 1 );
    }

    private void AddMeshes()
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
      if ( obj.SubmeshData.SubmeshList.Count == 0 )
        System.Diagnostics.Debugger.Break();

      foreach ( var submeshInfo in obj.SubmeshData.SubmeshList )
        AddSubMesh( obj, submeshInfo );
    }

    private void AddSubMesh( SaberObject obj, SubmeshInfo submeshInfo )
    {
      var mesh = MeshBuilder.Build( Context, obj, submeshInfo );
      var meshNode = Context.AddMesh( mesh );

      var transform = obj.Matrix.Value.ToMatrix4();
      transform.Transpose();
      meshNode.Transform = transform;
    }

    private void AddMaterials( IList<TextureListEntry> textures )
    {
      SetStatus( "Preparing Materials" );
      SetCompletedUnits( 0 );
      SetTotalUnits( textures.Count );
      SetIndeterminate( false );

      foreach ( var texture in textures )
        AddMaterial( texture );

      if ( Context.Scene.MaterialCount == 0 )
        Context.Scene.Materials.Add( new Material() { Name = "DefaultMaterial" } );
    }

    private void AddMaterial( string textureName )
    {
      var material = MaterialBuilder.Build( Context, textureName );
      Context.Scene.Materials.Add( material );
      IncreaseCompletedUnits( 1 );
    }

    #endregion

  }

}
