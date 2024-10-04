using System;
using System.Diagnostics;
using System.Windows.Input;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using Index.Common;
using Index.Domain.Assets.Meshes;
using Index.UI.ViewModels;
using Microsoft.VisualBasic.Logging;
using Prism.Commands;
using Prism.Ioc;
using SharpDX;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class MeshEditorViewModel : EditorViewModelBase<IMeshAsset>
  {

    #region Properties


    public Camera Camera { get; private set; }
    public EffectsManager EffectsManager { get; private set; }

    public SceneViewModel Scene { get; private set; }
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
      Scene = new SceneViewModel( container );

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
      Progress = new ProgressInfo();
      Progress.IsIndeterminate = true;
      Progress.Status = "Cleaning Up";
      ShowProgressOverlay = true;

      base.OnDisposing();

      Scene?.Dispose();
      Scene = null;

      EffectsManager?.ForceDispose();

      EffectsManager = null;
      Camera = null;
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
    }

    private void ZoomExtents()
    {
      const int MAX_RETRIES = 10;

      var boundsCalculated = false;
      BoundingBox bound = default;
      for ( var i = 0; i < MAX_RETRIES; i++ )
      {
        if ( !Scene.GroupModel.SceneNode.TryGetBound( out bound ) )
        {
          Debug.WriteLine( "ZoomExtents failed. Attempt {0}", i );

          Dispatcher.Invoke( () =>
          {
            Scene.ForceUpdateBounds();
          }, System.Windows.Threading.DispatcherPriority.ApplicationIdle );

          continue;
        }
        boundsCalculated = true;
        Debug.WriteLineIf( i > 0, $"Bounds successfully updated after {i} attempts.");
        break;
      }

      if(!boundsCalculated)
      {
        Serilog.Log.Error( "Failed to calculate bounds after 10 retries." +
          "If the viewport is empty, try reloading the asset." );
        return;
      }

      var maxWidth = Math.Max( Math.Max( bound.Width, bound.Height ), bound.Depth );
      var pos = bound.Center + new Vector3( 0, 0, maxWidth * 2 );

      Camera.Dispatcher.BeginInvoke( () =>
      {
        Camera.Position = pos.ToPoint3D();
        Camera.LookDirection = ( bound.Center - pos ).ToVector3D();
        Camera.UpDirection = Vector3.UnitY.ToVector3D();
      } );

      Debug.WriteLine( "ZoomExtents | MaxWidth: {0} | Pos: {1}", maxWidth, pos );
    }

    #endregion

  }

}
