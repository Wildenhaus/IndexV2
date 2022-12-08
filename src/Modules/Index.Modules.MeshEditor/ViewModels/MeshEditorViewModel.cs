using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using Index.Domain.Assets.Meshes;
using Index.Jobs;
using Index.UI.ViewModels;
using Prism.Commands;
using Prism.Ioc;
using SharpDX;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class MeshEditorViewModel : EditorViewModelBase<IMeshAsset>
  {

    #region Properties

    public Camera Camera { get; set; }
    public EffectsManager EffectsManager { get; set; }

    public SceneViewModel Scene { get; set; }

    public bool IsFlycamEnabled { get; set; }

    public ICommand ZoomExtentsCommand { get; set; }

    #endregion

    #region Constructor

    public MeshEditorViewModel( IContainerProvider container )
      : base( container )
    {
      Camera = new PerspectiveCamera() { FarPlaneDistance = 300000 };
      EffectsManager = new DefaultEffectsManager();

      ZoomExtentsCommand = new DelegateCommand( ZoomExtents );
    }

    #endregion

    #region Overrides

    protected override void OnInitializationJobCompleted( IJob job )
    {
      var typedJob = job as IJob<IMeshAsset>;
      var asset = typedJob.Result;

      Scene = SceneViewModel.Create( asset );
      ZoomExtents();
    }

    #endregion

    #region Private Methods

    private void ZoomExtents()
    {
      if ( !Scene.GroupModel.SceneNode.TryGetBound( out var bound ) )
        return;

      var maxWidth = Math.Max( Math.Max( bound.Width, bound.Height ), bound.Depth );
      var pos = bound.Center + new Vector3( 0, 0, maxWidth * 1.5f );

      Camera.Dispatcher.Invoke( () =>
      {
        Camera.Position = pos.ToPoint3D();
        Camera.LookDirection = ( bound.Center - pos ).ToVector3D();
        Camera.UpDirection = Vector3.UnitY.ToVector3D();
      } );
    }

    #endregion

  }

}
