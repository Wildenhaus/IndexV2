using System.Windows;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.Wpf.SharpDX;
using Index.Domain.Assets.Meshes;
using Index.UI.ViewModels;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class SceneViewModel : ViewModelBase
  {

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
      } );

      return sceneModel;
    }

  }

}
