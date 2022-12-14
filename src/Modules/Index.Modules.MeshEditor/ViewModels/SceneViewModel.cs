using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.SharpDX.Core.Model;
using HelixToolkit.SharpDX.Core.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using Index.Domain.Assets.Meshes;
using Index.UI.ViewModels;
using Serilog;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class SceneViewModel : ViewModelBase
  {

    #region Properties

    public SceneNodeGroupModel3D GroupModel { get; private set; }

    #endregion

    #region Constructor

    public SceneViewModel()
    {
      GroupModel = new SceneNodeGroupModel3D();
      ApplyTransforms();
    }

    #endregion

    #region Public Methods

    public void ApplyMeshAsset( IMeshAsset meshAsset )
    {
      using var importer = new Importer();
      importer.ToHelixToolkitScene( meshAsset.AssimpScene, out var helixScene );

      foreach ( var node in helixScene.Root.Traverse() )
      {
        if ( !( node is MeshNode meshNode ) )
          continue;

        meshNode.CullMode = SharpDX.Direct3D11.CullMode.Back;

        ApplyMaterialToNode( meshAsset, meshNode );

        if ( meshAsset.LodMeshNames.Contains( meshNode.Name ) )
          meshNode.Visible = false;
        else if ( meshAsset.VolumeMeshNames.Contains( meshNode.Name ) )
          meshNode.Visible = false;
      }

      GroupModel.Dispatcher.Invoke( () =>
      {
        GroupModel.AddNode( helixScene.Root );
        GroupModel.SceneNode.ForceUpdateTransformsAndBounds();
        GroupModel.GroupNode.ForceUpdateTransformsAndBounds();
      } );
    }

    #endregion

    #region Private Methods

    private void ApplyTransforms()
    {
      var transformGroup = new System.Windows.Media.Media3D.Transform3DGroup();

      var rotTransform = new System.Windows.Media.Media3D.RotateTransform3D();
      rotTransform.Rotation = new System.Windows.Media.Media3D.AxisAngleRotation3D(
        new System.Windows.Media.Media3D.Vector3D( 0, 1, 0 ), -90 );
      transformGroup.Children.Add( rotTransform );

      GroupModel.Transform = transformGroup;
    }

    private void ApplyMaterialToNode( IMeshAsset meshAsset, MeshNode meshNode )
    {
      var nodeMaterial = meshNode.Material as PhongMaterialCore;
      if ( nodeMaterial is null )
        return;

      if ( nodeMaterial.DiffuseMap is null && nodeMaterial.DiffuseMapFilePath is not null )
      {
        if ( !meshAsset.Textures.TryGetValue( nodeMaterial.DiffuseMapFilePath, out var texture ) )
        {
          Log.Logger.Error( "Failed to load texture {texName}.", nodeMaterial.Name );
          return;
        }

        nodeMaterial.DiffuseMap = TextureModel.Create( texture.Images[ 0 ].PreviewStream );
        nodeMaterial.UVTransform = new UVTransform( 0, 1, -1, 0, 0 );
      }
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      base.OnDisposing();

      GroupModel.GroupNode?.ForceDispose();
      GroupModel.SceneNode?.ForceDispose();
      GroupModel.Clear();
      GroupModel.Dispose();

      GroupModel = null;
    }

    #endregion

  }

}
