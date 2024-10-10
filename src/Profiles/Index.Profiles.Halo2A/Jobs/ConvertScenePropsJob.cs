using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assimp;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Profiles.Halo2A.Common;
using Index.Profiles.Halo2A.Meshes;
using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using Prism.Ioc;
using Serilog;

namespace Index.Profiles.Halo2A.Jobs
{

  public class ConvertScenePropsJob : JobBase
  {

    #region Properties

    protected IAssetManager AssetManager { get; set; }
    protected IAssetReference AssetReference { get; set; }
    protected IAssetLoadContext AssetLoadContext { get; set; }

    protected IJobManager JobManager { get; set; }

    protected SaberScene Scene { get; set; }
    protected SceneContext Context { get; set; }
    protected Dictionary<string, ITextureAsset> Textures { get; set; }

    protected ISet<string> LodMeshNames { get; set; }
    protected ISet<string> VolumeMeshNames { get; set; }

    protected List<CdListEntry> PropList { get; set; }

    #endregion

    #region Constructor

    public ConvertScenePropsJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      AssetManager = container.Resolve<IAssetManager>();
      JobManager = container.Resolve<IJobManager>();
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      SetIncludeUnitsInStatus();

      AssetReference = Parameters.Get<IAssetReference>();
      AssetLoadContext = Parameters.Get<IAssetLoadContext>();

      Scene = Parameters.Get<SaberScene>();
      Context = Parameters.Get<SceneContext>();
      Textures = Parameters.Get<Dictionary<string, ITextureAsset>>( "Textures" );

      LodMeshNames = Parameters.Get<ISet<string>>( "LodMeshSet" );
      VolumeMeshNames = Parameters.Get<ISet<string>>( "VolumeMeshSet" );
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
      var assetReference = AssetReference;
      var device = assetReference.Node.Device;
      var cdListNode = device.EnumerateFiles()
        .SingleOrDefault( x => Path.GetExtension( x.Name ) == ".h2a2_cd_list" );

      if ( cdListNode is null )
        return new HashSet<string>();

      var stream = cdListNode.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );
      PropList = Serializer.Deserialize<List<CdListEntry>>( reader );

      var toLoadSet = new HashSet<string>();
      foreach ( var prop in PropList )
      {
        var templateName = prop.Name.Replace( "|h", "__h_tpl" );
        templateName = $"{device.Root.Name}/{templateName}";
        toLoadSet.Add( templateName );
      }

      return toLoadSet;
    }

    private async Task<Dictionary<string, IMeshAsset>> LoadProps( ICollection<string> propsToLoad )
    {
      SetCompletedUnits( 0 );
      SetTotalUnits( propsToLoad.Count );
      SetIndeterminate( false );

      var loadedProps = new Dictionary<string, IMeshAsset>();
      foreach ( var templateName in propsToLoad )
      {
        if ( !AssetManager.TryGetAssetReference( typeof( IMeshAsset ), templateName, out var templateAssetReference ) )
        {
          //Log.Logger.Error( "Could not find prop: {propName}", templateName );
          IncreaseCompletedUnits( 1 );
          continue;
        }
        else
        {
          var propTemplateAsset = await AssetManager.LoadAssetAsync<IMeshAsset>( templateAssetReference, AssetLoadContext );

          if(propTemplateAsset is not null)
            loadedProps.Add( templateName, propTemplateAsset );
          IncreaseCompletedUnits( 1 );
        }
      }

      return loadedProps;
    }

    private void AddProps( Dictionary<string, IMeshAsset> loadedProps )
    {
      var deviceName = AssetReference.Node.Device.Root.Name;
      var groups = PropList.GroupBy( x => x.Name );
      foreach ( var group in groups )
      {
        var templateName = group.Key.Replace( "|h", "__h_tpl" );
        templateName = $"{deviceName}/{templateName}";
        if ( !loadedProps.TryGetValue( templateName, out var propAsset ) )
          continue;

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

        foreach ( var propReference in group )
        {
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

    #endregion

  }

}
