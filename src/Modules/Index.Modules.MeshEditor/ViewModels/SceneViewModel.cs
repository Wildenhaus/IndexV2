using System.Windows;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.SharpDX.Core.Model;
using HelixToolkit.SharpDX.Core.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using Index.Domain.Assets.Meshes;
using Index.UI.ViewModels;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class SceneViewModel : ViewModelBase
  {

    public Viewport3DX Viewport { get; set; }
    public Camera Camera { get; private set; }
    public EffectsManager EffectsManager { get; private set; }
    public SceneNodeGroupModel3D GroupModel { get; private set; }

    private SceneViewModel()
    {
      Application.Current.Dispatcher.Invoke( () =>
      {
        Camera = new PerspectiveCamera { FarPlaneDistance = 300000 };
        EffectsManager = new DefaultEffectsManager();
        GroupModel = new SceneNodeGroupModel3D();
      } );
    }

    public static SceneViewModel Create( IMeshAsset meshAsset )
    {
      var sceneModel = new SceneViewModel();

      var importer = new Importer();
      importer.ToHelixToolkitScene( meshAsset.AssimpScene, out var helixScene );

      Application.Current.Dispatcher.Invoke( () =>
      {
        sceneModel.GroupModel.AddNode( helixScene.Root );

        foreach ( var node in helixScene.Root.Traverse() )
        {
          if ( node is MeshNode meshNode )
            ApplyMaterialToNode( meshAsset, meshNode );
        }

        sceneModel.ApplyTransforms();
      } );

      return sceneModel;
    }

    private static void ApplyMaterialToNode( IMeshAsset meshAsset, MeshNode meshNode )
    {
      var nodeMaterial = meshNode.Material as PhongMaterialCore;
      if ( nodeMaterial is null )
        return;

      if ( nodeMaterial.DiffuseMapFilePath != null )
        nodeMaterial.DiffuseMap = TextureModel.Create( meshAsset.Textures[ nodeMaterial.DiffuseMapFilePath ].Images[ 0 ].PreviewStream );

      nodeMaterial.UVTransform = new UVTransform( 0, 1, -1, 0, 0 );
      nodeMaterial.EnableTessellation = true;
    }

    private void ApplyTransforms()
    {
      var transformGroup = new System.Windows.Media.Media3D.Transform3DGroup();

      var rotTransform = new System.Windows.Media.Media3D.RotateTransform3D();
      rotTransform.Rotation = new System.Windows.Media.Media3D.AxisAngleRotation3D(
        new System.Windows.Media.Media3D.Vector3D( 0, 1, 0 ), -90 );
      transformGroup.Children.Add( rotTransform );

      GroupModel.Transform = transformGroup;
    }

  }

}
