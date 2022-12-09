using System.Diagnostics;
using Assimp;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Profiles.HaloCEA.Assets;
using Index.Profiles.HaloCEA.Common;
using Index.Profiles.HaloCEA.Meshes;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;
using Prism.Ioc;
using Serilog;
using Serilog.Core;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class LoadSceneJob : LoadGeometryJobBase<CEASceneAsset>
  {

    #region Properties

    protected SaberScene Scene { get; set; }

    #endregion

    #region Constructor

    public LoadSceneJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Overrides

    protected override Task<SceneContext> CreateSceneContext( IAssetReference assetReference )
    {
      return Task.Run( () =>
      {
        var stream = assetReference.Node.Open();
        var reader = new NativeReader( stream, Endianness.LittleEndian );

        Scene = SaberScene.Deserialize( reader, new SerializationContext() );

        var context = SceneContext.Create( Scene.Objects );

        return context;
      } );
    }

    protected override IList<TextureListEntry> GetTextureList()
      => Scene.TextureList;

    protected override CEASceneAsset CreateAsset( Scene assimpScene )
    {
      var asset = new CEASceneAsset( AssetReference );
      asset.AssimpScene = assimpScene;
      asset.Textures = Textures;

      return asset;
    }

    protected override async Task CreateScene( SceneContext context )
    {
      await base.CreateScene( context );

      await AddProps();
      AddLights();
    }

    #endregion

    #region Private Methods

    private async Task AddProps()
    {
      SetIndeterminate();
      SetStatus( "Loading Scene Props" );

      var deviceName = AssetReference.Node.Device.Root.Name;
      var sceneRoot = Context.RootObject;
      var sceneRootNode = Context.Nodes[ sceneRoot.ObjectInfo.Id ];

      var propsToLoad = new HashSet<string>();
      foreach ( var propReference in Scene.Props )
      {
        var templateName = propReference.PropInfo.TemplateName;
        propsToLoad.Add( $"{deviceName}/{templateName}" );
      }

      SetCompletedUnits( 0 );
      SetTotalUnits( propsToLoad.Count );
      SetIndeterminate( false );

      var loadedProps = new Dictionary<string, IMeshAsset>();
      foreach ( var templateName in propsToLoad )
      {
        if ( !AssetManager.TryGetAssetReference( typeof( IMeshAsset ), templateName, out var templateAssetRef ) )
        {
          Log.Logger.Error( "Could not find prop: {propName}", templateName );
          continue;
        }

        var job = AssetManager.LoadAsset<IMeshAsset>( templateAssetRef );
        await job.Completion;

        loadedProps.Add( templateName, job.Result );
        IncreaseCompletedUnits( 1 );
      }

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

    private void AddLights()
    {
      SetStatus( "Loading Lights" );
      SetCompletedUnits( 0 );
      SetTotalUnits( Scene.Lights.Count );

      foreach ( var sceneLight in Scene.Lights )
      {
        var lightName = sceneLight.Name;

        var light = new Light() { Name = lightName };
        var lightNode = new Node( lightName, Context.RootNode );
        Context.RootNode.Children.Add( lightNode );

        var matrix = sceneLight.Matrix.ToAssimp();
        matrix.Transpose();
        lightNode.Transform = matrix;

        var pos = sceneLight.Matrix;
        light.Position = new Vector3D( pos.M41, pos.M42, pos.M43 );

        switch ( sceneLight.LightInfo.LightType )
        {
          case 0:
            light.LightType = LightSourceType.Point;
            break;
          case 1:
            light.LightType = LightSourceType.Spot;
            break;
          case 4:
            light.LightType = LightSourceType.Directional;
            break;
        }

        light.ColorDiffuse = new Color3D(
          sceneLight.Color.X,
          sceneLight.Color.Y,
          sceneLight.Color.Z );
        light.ColorSpecular = light.ColorDiffuse;
        light.ColorAmbient = light.ColorDiffuse;

        light.AngleOuterCone = sceneLight.Data_0285.Unk_00;
        light.AngleInnerCone = sceneLight.Data_0285.Unk_01;
        light.Up = new Vector3D( 0, 0, -1 );

        Context.Scene.Lights.Add( light );
        IncreaseCompletedUnits( 1 );
      }
    }

    #endregion

  }

}
