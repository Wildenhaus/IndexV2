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
using Serilog;
using SharpDX;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class MeshEditorViewModel : EditorViewModelBase<IMeshAsset>
  {

    #region Properties


    public Camera Camera { get; }
    public EffectsManager EffectsManager { get; }

    public SceneViewModel Scene { get; }
    public bool IsFlycamEnabled { get; set; }

    public double MinMoveSpeed { get; set; }
    public double MoveSpeed { get; set; }
    public double MaxMoveSpeed { get; set; }

    public ICommand ZoomExtentsCommand { get; set; }

    #endregion

    #region Constructor

    public MeshEditorViewModel( IContainerProvider container )
      : base( container )
    {
      Camera = new PerspectiveCamera() { FarPlaneDistance = 300000 };
      EffectsManager = new DefaultEffectsManager();
      Scene = new SceneViewModel();

      ZoomExtentsCommand = new DelegateCommand( ZoomExtents );
    }

    #endregion

    #region Overrides

    protected override void OnAssetLoaded( IMeshAsset asset )
    {
      Scene.ApplyMeshAsset( asset );
      RecalculateMoveSpeed();
      ZoomExtents();
    }

    protected override void OnDisposing()
    {
      base.OnDisposing();
      Scene?.Dispose();
      EffectsManager?.Dispose();
    }

    #endregion

    #region Private Methods

    private void RecalculateMoveSpeed()
    {
      if ( !Scene.GroupModel.SceneNode.TryGetBound( out var bound ) )
        return;

      const double BASELINE_MIN_SPEED = 0.00001;
      const double BASELINE_DEFAULT_SPEED = 0.01;
      const double BASELINE_MAX_SPEED = 0.5;

      float maxDim;
      maxDim = Math.Max( bound.Width, bound.Height );
      maxDim = Math.Max( maxDim, bound.Depth );
      var coef = maxDim / 5;

      MinMoveSpeed = BASELINE_MIN_SPEED * coef;
      MoveSpeed = BASELINE_DEFAULT_SPEED * coef;
      MaxMoveSpeed = BASELINE_MAX_SPEED * coef;
      Serilog.Log.Information( "Bounds {min} {def} {max}", bound.Width, bound.Height, bound.Depth );
      Serilog.Log.Information( "MoveSpeed {min} {def} {max}", MinMoveSpeed, MoveSpeed, MaxMoveSpeed );
    }

    private void ZoomExtents()
    {
      if ( !Scene.GroupModel.SceneNode.TryGetBound( out var bound ) )
        return;

      Log.Logger.Information( "{bound}", bound );
      var maxWidth = Math.Max( Math.Max( bound.Width, bound.Height ), bound.Depth );
      var pos = bound.Center + new Vector3( 0, 0, maxWidth * 2 );

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
