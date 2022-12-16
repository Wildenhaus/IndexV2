using Assimp;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using Prism.Ioc;
using Serilog;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class ConvertScenePropsJob : JobBase
  {

    protected IAssetManager AssetManager { get; set; }
    protected IAssetReference AssetReference { get; set; }
    protected IAssetLoadContext AssetLoadContext { get; set; }

    protected IJobManager JobManager { get; set; }

    protected SaberScene Scene { get; set; }
    protected SceneContext Context { get; set; }
    protected Dictionary<string, ITextureAsset> Textures { get; set; }

    public ConvertScenePropsJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      AssetManager = container.Resolve<IAssetManager>();
      JobManager = container.Resolve<IJobManager>();
    }

    protected override async Task OnInitializing()
    {
      SetIncludeUnitsInStatus();

      AssetReference = Parameters.Get<IAssetReference>();
      AssetLoadContext = Parameters.Get<IAssetLoadContext>();

      Scene = Parameters.Get<SaberScene>();
      Context = Parameters.Get<SceneContext>();
      Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );
    }

    protected override async Task OnExecuting()
    {
      SetStatus( "Loading Scene Props" );
      SetIndeterminate();

      var propsToLoad = GatherProps();
      var loadedProps = await LoadProps( propsToLoad );
      AddProps( loadedProps );
    }

    private HashSet<string> GatherProps()
    {
      var deviceName = AssetReference.Node.Device.Root.Name;

      var propsToLoad = new HashSet<string>();
      foreach ( var propReference in Scene.Props )
      {
        var templateName = propReference.PropInfo.TemplateName;
        propsToLoad.Add( $"{deviceName}/{templateName}" );
      }

      return propsToLoad;
    }

    private async Task<Dictionary<string, IMeshAsset>> LoadProps( ICollection<string> propsToLoad )
    {
      SetCompletedUnits( 0 );
      SetTotalUnits( propsToLoad.Count );
      SetIndeterminate( false );

      var loadedProps = new Dictionary<string, IMeshAsset>();
      foreach ( var templateName in propsToLoad )
      {
        if ( !AssetManager.TryGetAssetReference( typeof( IMeshAsset ), templateName, out var templateAssetRef ) )
        {
          var embeddedPropTemplate = await TryLoadEmbeddedPropTemplate( templateName );
          if ( embeddedPropTemplate is null )
          {
            Log.Logger.Error( "Could not find prop: {propName}", templateName );
            continue;
          }

          loadedProps.Add( templateName, embeddedPropTemplate );
          IncreaseCompletedUnits( 1 );
        }
        else
        {
          var propTemplateAsset = await AssetManager.LoadAssetAsync<IMeshAsset>( templateAssetRef, AssetLoadContext );

          loadedProps.Add( templateName, propTemplateAsset );
          IncreaseCompletedUnits( 1 );
        }
      }

      return loadedProps;
    }

    private async Task<IMeshAsset> TryLoadEmbeddedPropTemplate( string templateName )
    {
      var tplName = templateName.Substring( templateName.IndexOf( '/' ) + 1 );
      var tplData = Scene.TemplateList.FirstOrDefault( x => x.TemplateInfo.Value.Name == tplName );
      if ( tplData is null )
        return null;

      var embeddedPropContext = SceneContext.Create( tplData.Objects );

      var parameters = new ParameterCollection();
      parameters.Set( AssetLoadContext );
      parameters.Set( embeddedPropContext );
      parameters.Set( tplData );
      parameters.Set( tplData.TextureList ?? new TextureList() );
      parameters.Set( "Textures", Textures );

      var job = JobManager.StartJob<LoadEmbeddedTemplateJob>( parameters );
      await job.Completion;

      return job.Result;
    }

    private void AddProps( Dictionary<string, IMeshAsset> loadedProps )
    {
      var deviceName = AssetReference.Node.Device.Root.Name;

      var groups = Scene.Props.GroupBy( x => x.PropInfo.TemplateName );
      foreach ( var group in groups )
      {
        var templateName = $"{deviceName}/{group.Key}";
        if ( !loadedProps.TryGetValue( templateName, out var propAsset ) )
          continue;

        foreach ( var pair in propAsset.Textures )
          Textures.TryAdd( pair.Key, pair.Value );

        var propScene = propAsset.AssimpScene;

        var materialLookup = new Dictionary<int, int>();
        for ( var i = 0; i < propScene.MaterialCount; i++ )
        {
          var material = propScene.Materials[ i ];
          var newMaterialIndex = Context.AddMaterial( material );
          materialLookup.Add( i, newMaterialIndex );
        }

        var meshLookup = new Dictionary<int, int>();
        for ( var i = 0; i < propScene.MeshCount; i++ )
        {
          var mesh = propScene.Meshes[ i ];
          var newMeshIndex = Context.Scene.MeshCount;
          Context.Scene.Meshes.Add( mesh );
          meshLookup.Add( i, newMeshIndex );

          if ( materialLookup.TryGetValue( mesh.MaterialIndex, out var newMaterialIndex ) )
            mesh.MaterialIndex = newMaterialIndex;
          else if ( Textures.Count >= mesh.MaterialIndex )
            mesh.MaterialIndex = mesh.MaterialIndex; // Should assert that this is an embedded template
          else
            System.Diagnostics.Debugger.Break();
        }

        foreach ( var propReference in group )
        {
          var instanceName = propReference.PropInfo.InstanceName;
          var instanceMatrix = propReference.PropInfo.Matrix;
          var instanceParentObject = Context.Objects[ propReference.PropInfo.Data_01BD ];
          var instanceParentNode = Context.Nodes[ instanceParentObject.ObjectInfo.Id ];

          var matrix = instanceMatrix.ToAssimp();
          matrix.Transpose();

          var propNode = new Node( instanceName, instanceParentNode );
          instanceParentNode.Children.Add( propNode );
          propNode.Transform = matrix;

          AddPropNode( propScene.RootNode, propNode, meshLookup );
        }
      }
    }

    private void AddPropNode( Node originalNode, Node parent, Dictionary<int, int> meshLookup )
    {
      var newNode = new Node( originalNode.Name, parent );
      parent.Children.Add( newNode );
      newNode.Transform = originalNode.Transform;

      foreach ( var oldMeshIndex in originalNode.MeshIndices )
        newNode.MeshIndices.Add( meshLookup[ oldMeshIndex ] );

      foreach ( var child in originalNode.EnumerateChildren() )
        AddPropNode( child, newNode, meshLookup );
    }

  }

}
