using System.Windows;
using HelixToolkit.Wpf.SharpDX;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.Modules.MeshEditor.Views;
using Index.UI.ViewModels;
using Prism.Ioc;
using Prism.Regions;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class MeshEditorViewModel : EditorViewModelBase<IMeshAsset>
  {

    #region Properties

    private IRegionManager _regionManager;
    public SceneViewModel Scene { get; private set; }
    public Viewport3DX Viewport { get; set; }

    #endregion

    #region Constructor

    public MeshEditorViewModel( IContainerProvider container )
      : base( container )
    {
      _regionManager = container.Resolve<IRegionManager>();
    }

    #endregion

    #region Overrides

    protected override void OnInitializationJobCompleted( IJob job )
    {
      var typedJob = job as IJob<IMeshAsset>;
      var asset = typedJob.Result;

      Scene = SceneViewModel.Create( asset );

      Application.Current.Dispatcher.Invoke( () =>
      {
        Scene.Camera.ZoomExtents( Viewport );
        //var region = _regionManager.Regions[ "ModelViewerContentRegion" ];
        //var meshView = Container.Resolve<MeshView>();

        //region.Add( meshView );
      } );
    }

    #endregion

  }

}
