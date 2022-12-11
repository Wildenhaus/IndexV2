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

    private void ZoomExtents()
    {
      if ( !Scene.GroupModel.SceneNode.TryGetBound( out var bound ) )
        return;

      Log.Logger.Information( "{bound}", bound );
      var maxWidth = Math.Max( Math.Max( bound.Width, bound.Height ), bound.Depth );
      var pos = bound.Center + new Vector3( 0, 0, maxWidth );

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
