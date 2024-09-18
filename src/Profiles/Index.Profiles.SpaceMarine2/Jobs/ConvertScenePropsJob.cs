using Assimp;
using Index.Domain.Assets.Meshes;
using Index.Domain.Assets.Textures;
using Index.Domain.Assets;
using Index.Jobs;
using Prism.Ioc;
using LibSaber.SpaceMarine2.Structures;
using Index.Profiles.SpaceMarine2.Meshes;
using LibSaber.IO;
using LibSaber.SpaceMarine2.Serialization;
using Index.Profiles.SpaceMarine2.Common;
using Index.Domain.FileSystem;
using Serilog;

namespace Index.Profiles.SpaceMarine2.Jobs
{

  public class ConvertScenePropsJob : JobBase
  {

    #region Properties

    protected IFileSystem FileSystem { get; set; }
    protected IAssetManager AssetManager { get; set; }
    protected IAssetReference AssetReference { get; set; }
    protected IAssetLoadContext AssetLoadContext { get; set; }

    protected IJobManager JobManager { get; set; }

    protected scnSCENE Scene { get; set; }
    protected SceneContext Context { get; set; }
    protected Dictionary<string, ITextureAsset> Textures { get; set; }

    protected ISet<string> LodMeshNames { get; set; }
    protected ISet<string> VolumeMeshNames { get; set; }

    protected cdLIST CdList { get; set; }
    protected ClassList ClassList { get; set; }

    #endregion

    #region Constructor

    public ConvertScenePropsJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      AssetManager = container.Resolve<IAssetManager>();
      JobManager = container.Resolve<IJobManager>();
      FileSystem = container.Resolve<IFileSystem>();
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      SetIncludeUnitsInStatus();

      AssetReference = Parameters.Get<IAssetReference>();
      AssetLoadContext = Parameters.Get<IAssetLoadContext>();

      Scene = Parameters.Get<scnSCENE>();
      Context = Parameters.Get<SceneContext>();
      Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );

      LodMeshNames = Parameters.Get<ISet<string>>( "LodMeshSet" );
      VolumeMeshNames = Parameters.Get<ISet<string>>( "VolumeMeshSet" );

      CdList = Parameters.Get<cdLIST>();
      ClassList = Parameters.Get<ClassList>();
    }

    protected override async Task OnExecuting()
    {
      SetStatus( "Loading Scene Props" );
      SetIndeterminate();

      var propsToLoad = GatherProps();
      if ( propsToLoad.Count == 0 )
        return;

      var loadedProps = await LoadProps( propsToLoad );
      AddProps( loadedProps );
    }

    #endregion

    #region Private Methods

    private HashSet<string> GatherProps()
    {
      if ( CdList is null || ClassList is null )
        return new HashSet<string>();

      var tplNames = ClassList.TplLookup.Select( x => x.Value + ".tpl" ).ToHashSet();
      foreach ( var entry in CdList )
      {
        foreach ( var tpl in entry.NameTpls )
          tplNames.Add( tpl + ".tpl" );
      }

      return tplNames;
    }

    private async Task<Dictionary<string, IMeshAsset>> LoadProps( ICollection<string> propsToLoad )
    {
      SetCompletedUnits( 0 );
      SetTotalUnits( propsToLoad.Count );
      SetIndeterminate( false );

      var loadedProps = new Dictionary<string, IMeshAsset>();
      //foreach ( var templateName in propsToLoad )
      //{
      //  if ( !AssetManager.TryGetAssetReference( typeof( IMeshAsset ), templateName, out var templateAssetReference ) )
      //  {
      //    //Log.Logger.Error( "Could not find prop: {propName}", templateName );
      //    IncreaseCompletedUnits( 1 );
      //    continue;
      //  }
      //  else
      //  {
      //    var propTemplateAsset = await AssetManager.LoadAssetAsync<IMeshAsset>( templateAssetReference, AssetLoadContext );

      //    loadedProps.Add( templateName, propTemplateAsset );
      //    IncreaseCompletedUnits( 1 );
      //  }
      //}

      var loadTasks = propsToLoad.Select( templateName => Task.Run( async () =>
      {
        if ( !AssetManager.TryGetAssetReference( typeof( IMeshAsset ), templateName, out var templateAssetReference ) )
        {
          Log.Logger.Error( "Could not find prop: {propName}", templateName );
          IncreaseCompletedUnits( 1 );
          return;
        }
        else
        {
          var propTemplateAsset = await AssetManager.LoadAssetAsync<IMeshAsset>( templateAssetReference, AssetLoadContext );
          if ( propTemplateAsset is null )
            return;

          lock ( loadedProps )
            loadedProps.Add( templateName, propTemplateAsset );

          IncreaseCompletedUnits( 1 );
        }
      } ) );

      await Task.WhenAll( loadTasks );

      return loadedProps;
    }

    private void AddProps( Dictionary<string, IMeshAsset> loadedProps )
    {
      var deviceName = AssetReference.Node.Device.Root.Name;
      var tplLookup = ClassList.TplLookup;
      foreach ( var cdEntry in CdList )
      {
        var type = cdEntry.__type;

        if ( !tplLookup.TryGetValue( type, out var templateName ) )
        {
          //Log.Logger.Information( "Tpl lookup doesn't contain {0}.", templateName );
          templateName = cdEntry.NameTpls.FirstOrDefault();
        }
        if ( templateName is null )
          continue;

        if ( !loadedProps.TryGetValue( templateName + ".tpl", out var propAsset ) )
        {
          Log.Logger.Information( "Could not find tpl {0}.", templateName );
          continue;
        }

        if ( propAsset is null )
        {
          Log.Logger.Information( "propAsset is null." );
          continue;
        }

        foreach ( var pair in propAsset.Textures )
          Textures.TryAdd( pair.Key, pair.Value );

        foreach ( var lodName in propAsset.LodMeshNames )
          LodMeshNames.Add( lodName );
        foreach ( var volumeName in propAsset.VolumeMeshNames )
          VolumeMeshNames.Add( volumeName );

        var propScene = propAsset.AssimpScene;

        var materialLookup = new Dictionary<int, int>();
        for ( var i = 0; i < propScene.MaterialCount; i++ )
        {
          var material = propScene.Materials[ i ];
          var newMaterialIndex = Context.Scene.Materials.Count;
          Context.Scene.Materials.Add( material );
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
          else
            mesh.MaterialIndex = 0;
        }

        var propReference = cdEntry;
        var instanceName = propReference.Name;
        var instanceMatrix = propReference.Matrix;

        var matrix = instanceMatrix.ToAssimp();
        matrix.Transpose();

        var propNode = new Node( instanceName, Context.RootNode );
        Context.RootNode.Children.Add( propNode );
        propNode.Transform = matrix;

        AddPropNode( propScene.RootNode, propNode, meshLookup );
      }
    }

    private void AddPropNode( Node originalNode, Node parent, Dictionary<int, int> meshLookup )
    {
      foreach ( var meshNode in originalNode.Children.Where( x => x.HasMeshes ) )
      {
        foreach ( var oldMeshIndex in originalNode.MeshIndices )
          parent.MeshIndices.Add( meshLookup[ oldMeshIndex ] );
      }

      //var newNode = new Node( originalNode.Name, parent );
      //parent.Children.Add( newNode );
      //newNode.Transform = originalNode.Transform;

      //foreach ( var oldMeshIndex in originalNode.MeshIndices )
      //  newNode.MeshIndices.Add( meshLookup[ oldMeshIndex ] );

      //foreach ( var child in originalNode.EnumerateChildren() )
      //  AddPropNode( child, newNode, meshLookup );
    }

    #endregion


  }

}
