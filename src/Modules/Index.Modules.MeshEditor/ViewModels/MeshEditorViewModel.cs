using System.Windows;
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
        _regionManager.RegisterViewWithRegion( "ModelViewerContentRegion", typeof( MeshView ) );
      } );
    }

    #endregion

  }

}
