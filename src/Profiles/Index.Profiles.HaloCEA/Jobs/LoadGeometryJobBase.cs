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

  public abstract class LoadGeometryJobBase<TAsset> : JobBase<TAsset>
    where TAsset : class, IMeshAsset
  {

    #region Constants

    private static readonly string[] VOLUME_NAME_HINTS = new string[]
    {
      ".sh_cast",
      "sh_cast",
      "cin_",
      "start1",
      ".pCube",
      "path_",
      "add_",
      ".pPlane",

    };

    #endregion

    #region Properties

    public IAssetManager AssetManager { get; }

    protected SceneContext Context { get; private set; }

    protected IAssetReference AssetReference { get; }

    protected Dictionary<string, ITextureAsset> Textures { get; private set; }

    #endregion

    #region Constructor

    protected LoadGeometryJobBase( IContainerProvider container, IParameterCollection parameters = null )
      : base( container, parameters )
    {
      AssetManager = container.Resolve<IAssetManager>();

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
      await CreateScene( Context );

      SetStatus( "Preparing Viewer" );
      SetIndeterminate();

      var asset = CreateAsset( Context.Scene );
      CreateLodMeshSet( asset, Context.Scene );
      CreateVolumeMeshSet( asset, Context.Scene );

      SetResult( asset );
    }

    #endregion

    #region Abstract Methods

    protected abstract Task<SceneContext> CreateSceneContext( IAssetReference assetReference );

    protected abstract IList<TextureListEntry> GetTextureList();

    protected abstract TAsset CreateAsset( Scene assimpScene );

    #endregion

    #region Virtual Methods

    protected virtual async Task CreateScene( SceneContext context )
    {
      var textureList = GetTextureList();
      Textures = await GatherTextures( textureList );
      AddMaterials( textureList );

      AddNodes();
      AddMeshes();
    }

    protected virtual bool IsLodMesh( string meshName )
      => meshName.Contains( ".lod." );

    protected virtual bool IsVolumeMesh( string meshName )
    {
      foreach ( var hint in VOLUME_NAME_HINTS )
        if ( meshName.StartsWith( hint, StringComparison.InvariantCultureIgnoreCase ) )
          return true;

      return false;
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

    private async Task<Dictionary<string, ITextureAsset>> GatherTextures( IList<TextureListEntry> textureNames )
    {
      SetStatus( "Loading Textures" );
      SetIndeterminate();

      var toLoadSet = new HashSet<IAssetReference>();
      var textureAssetReferences = AssetManager.GetAssetReferencesOfType<ITextureAsset>();

      foreach ( var textureName in textureNames )
      {
        var matches = textureAssetReferences.Where( x => x.AssetName.StartsWith( textureName ) );
        foreach ( var match in matches )
          toLoadSet.Add( match );
      }

      SetCompletedUnits( 0 );
      SetTotalUnits( toLoadSet.Count );
      SetIndeterminate( false );

      var loadedTextures = new Dictionary<string, ITextureAsset>();
      foreach ( var assetToLoad in toLoadSet )
      {
        var loadJob = AssetManager.LoadAsset<ITextureAsset>( assetToLoad );
        await loadJob.Completion;

        var texture = loadJob.Result;
        loadedTextures.Add( texture.AssetName, texture );
        IncreaseCompletedUnits( 1 );
      }

      return loadedTextures;
    }

    private void CreateLodMeshSet( IMeshAsset meshAsset, Scene scene )
      => EvaluateMesh( meshAsset.LodMeshNames, scene.RootNode, IsLodMesh );

    private void CreateVolumeMeshSet( IMeshAsset meshAsset, Scene scene )
      => EvaluateMesh( meshAsset.VolumeMeshNames, scene.RootNode, IsVolumeMesh );

    private void EvaluateMesh( ISet<string> set, Node node, Func<string, bool> evaluatorFunc )
    {
      var name = node.Name;
      if ( evaluatorFunc( name ) )
        set.Add( name );

      foreach ( var child in EnumerateSceneNodeChildren( node ) )
        EvaluateMesh( set, child, evaluatorFunc );
    }

    private IEnumerable<Node> EnumerateSceneNodeChildren( Node node )
    {
      foreach ( var child in node.Children )
        yield return child;
    }

    #endregion

  }

}
