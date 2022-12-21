using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.Wpf.SharpDX;
using Index.UI.Common;
using Index.Utilities;
using Prism.Commands;
using SharpDX;
using Camera = HelixToolkit.Wpf.SharpDX.Camera;
using Point = System.Windows.Point;
using Quaternion = System.Windows.Media.Media3D.Quaternion;
using Vector3 = SharpDX.Vector3;

namespace Index.Modules.MeshEditor.Views
{

  public sealed partial class MeshView : UserControl, IDisposable
  {

    #region Data Members

    private bool _isMouseCaptured;
    private Point _lastMousePos;

    private bool _isFrameRendered;
    private ManualResetEvent _isFocusedEvent;

    private Thread _monitorThread;
    private CancellationTokenSource _monitorThreadCts;

    private double _moveSpeed;
    private double _minMoveSpeed;
    private double _maxMoveSpeed;

    private double _newMoveSpeed;
    private ActionThrottler _moveSpeedThrottler;

    #endregion

    #region Properties

    public static readonly DependencyProperty CameraProperty = DependencyProperty.Register(
      nameof( Camera ),
      typeof( Camera ),
      typeof( MeshView ) );

    public Camera Camera
    {
      get => ( Camera ) GetValue( CameraProperty );
      set => SetValue( CameraProperty, value );
    }

    public static readonly DependencyProperty EffectsManagerProperty = DependencyProperty.Register(
      nameof( EffectsManager ),
      typeof( EffectsManager ),
      typeof( MeshView ) );

    public EffectsManager EffectsManager
    {
      get => ( EffectsManager ) GetValue( EffectsManagerProperty );
      set => SetValue( EffectsManagerProperty, value );
    }

    public static readonly DependencyProperty IsFlycamEnabledProperty = DependencyProperty.Register(
      nameof( IsFlycamEnabled ),
      typeof( bool ),
      typeof( MeshView ) );

    public bool IsFlycamEnabled
    {
      get => ( bool ) GetValue( IsFlycamEnabledProperty );
      set => SetValue( IsFlycamEnabledProperty, value );
    }

    public static readonly DependencyProperty ModelProperty = DependencyProperty.Register(
      nameof( Model ),
      typeof( SceneNodeGroupModel3D ),
      typeof( MeshView ),
      new PropertyMetadata( null, OnModelPropertyChanged ) );

    public SceneNodeGroupModel3D Model
    {
      get => ( SceneNodeGroupModel3D ) GetValue( ModelProperty );
      set => SetValue( ModelProperty, value );
    }

    public static readonly DependencyProperty MinMoveSpeedProperty = DependencyProperty.Register(
      nameof( MinMoveSpeed ),
      typeof( double ),
      typeof( MeshView ),
      new PropertyMetadata( 0.0001d, OnMoveSpeedPropertyChanged ) );

    public double MinMoveSpeed
    {
      get => ( double ) GetValue( MinMoveSpeedProperty );
      set => SetValue( MinMoveSpeedProperty, value );
    }

    public static readonly DependencyProperty MoveSpeedProperty = DependencyProperty.Register(
      nameof( MoveSpeed ),
      typeof( double ),
      typeof( MeshView ),
      new PropertyMetadata( 0.001d, OnMoveSpeedPropertyChanged ) );

    public double MoveSpeed
    {
      get => ( double ) GetValue( MoveSpeedProperty );
      set => SetValue( MoveSpeedProperty, value );
    }

    public static readonly DependencyProperty MaxMoveSpeedProperty = DependencyProperty.Register(
      nameof( MaxMoveSpeed ),
      typeof( double ),
      typeof( MeshView ),
      new PropertyMetadata( 1.0d, OnMoveSpeedPropertyChanged ) );

    public double MaxMoveSpeed
    {
      get => ( double ) GetValue( MaxMoveSpeedProperty );
      set => SetValue( MaxMoveSpeedProperty, value );
    }

    #endregion

    #region Constructor

    public MeshView()
    {
      InitializeComponent();

      RemoveDirectionalViewKeyBindings();
      InitializeEventHandlers();
      InitializeMonitorThread();
    }

    #endregion

    #region Private Methods

    private void InitializeEventHandlers()
    {
      Viewport.PreviewKeyDown += OnViewportPreviewKeyDown;
      Viewport.OnRendered += OnFrameRendered;
    }

    private void DisposeEventHandlers()
    {
      Viewport.PreviewKeyDown -= OnViewportPreviewKeyDown;
      Viewport.OnRendered -= OnFrameRendered;
    }

    private void InitializeMonitorThread()
    {
      _moveSpeedThrottler = new ActionThrottler( () =>
      {
        Dispatcher.Invoke( () =>
        {
          _moveSpeed = _newMoveSpeed;
          MoveSpeed = _moveSpeed;
        }, DispatcherPriority.Send );
      }, 100 );

      _isFocusedEvent = new ManualResetEvent( false );
      _monitorThreadCts = new CancellationTokenSource();
      _monitorThread = new Thread( () => MonitorThread( _monitorThreadCts.Token ) );

      _monitorThread.Start();
    }

    private void DisposeMonitorThread()
    {
      _monitorThreadCts.Cancel();
      _monitorThread.Join();

      _monitorThreadCts.Dispose();
      _isFocusedEvent.Dispose();
    }

    private void RemoveDirectionalViewKeyBindings()
    {
      var toRemove = InputBindings.OfType<KeyBinding>().ToArray();
      foreach ( var binding in toRemove )
        InputBindings.Remove( binding );
    }

    private void MonitorThread( CancellationToken cancellationToken )
    {
      var waitHandles = new WaitHandle[]
      {
        cancellationToken.WaitHandle,
        _isFocusedEvent
      };

      while ( true )
      {
        WaitHandle.WaitAny( waitHandles );
        if ( cancellationToken.IsCancellationRequested )
          return;

        if ( !_isFrameRendered )
          continue;

        HandleKeyboard();
        HandleMouse();
      }
    }

    private void HandleKeyboard()
    {
      var wPressed = Win32.IsKeyPressed( WinKeys.W );
      var aPressed = Win32.IsKeyPressed( WinKeys.A );
      var sPressed = Win32.IsKeyPressed( WinKeys.S );
      var dPressed = Win32.IsKeyPressed( WinKeys.D );
      var qPressed = Win32.IsKeyPressed( WinKeys.Q );
      var ePressed = Win32.IsKeyPressed( WinKeys.E );
      var rPressed = Win32.IsKeyPressed( WinKeys.R );
      var fPressed = Win32.IsKeyPressed( WinKeys.F );
      var shiftPressed = Win32.IsKeyPressed( WinKeys.Shift );

      if ( !( wPressed || aPressed || sPressed || dPressed
          || qPressed || ePressed
          || rPressed || fPressed ) )
        return;

      var delta = new Vector3D();

      var speed = _moveSpeed;
      if ( shiftPressed )
        speed *= 2;

      if ( wPressed )
        delta.Z += speed;
      if ( aPressed )
        delta.X -= speed;
      if ( sPressed )
        delta.Z -= speed;
      if ( dPressed )
        delta.X += speed;
      if ( qPressed )
        delta.Y -= speed;
      if ( ePressed )
        delta.Y += speed;

      if ( rPressed || fPressed )
      {
        if ( rPressed )
        {
          _newMoveSpeed = Math.Clamp( _moveSpeed * 2, _minMoveSpeed, _maxMoveSpeed );
          _moveSpeedThrottler.Execute();
        }
        if ( fPressed )
        {
          _newMoveSpeed = Math.Clamp( _moveSpeed / 2, _minMoveSpeed, _maxMoveSpeed );
          _moveSpeedThrottler.Execute();
        }
      }

      _isFrameRendered = false;
      Dispatcher.Invoke( () => Viewport.AddMoveForce( delta ), DispatcherPriority.Send );
    }

    private void HandleMouse()
    {
      if ( !_isMouseCaptured )
        return;

      var lastPos = _lastMousePos;
      if ( !Win32.GetCursorPosition( out var mousePos ) )
        return;

      var dX = mousePos.X - lastPos.X;
      var dY = mousePos.Y - lastPos.Y;

      Vector3D lookDir;
      Vector3D upDir = new Vector3D( 0, 1, 0 );
      Dispatcher.Invoke( () => { lookDir = Camera.LookDirection; }, DispatcherPriority.Send );
      lookDir.Normalize();

      var rightDir = Vector3D.CrossProduct( lookDir, upDir );
      rightDir.Normalize();

      var q1 = new Quaternion( -upDir, 0.5 * dX );
      var q2 = new Quaternion( -rightDir, 0.5 * dY );
      var q = q1 * q2;

      var m = new Matrix3D();
      m.Rotate( q );

      Win32.SetCursorPosition( lastPos );
      var newLookDir = m.Transform( lookDir );
      var newUpDir = m.Transform( upDir );

      Dispatcher.Invoke( () =>
      {
        Camera.LookDirection = newLookDir;
        Camera.UpDirection = newUpDir;
      }, DispatcherPriority.Send );
    }

    private void RecalculateMoveSpeed()
    {
      if ( Model is null )
        return;

      if ( !TryGetSceneBounds( out var bound ) )
        return;

      const double BASELINE_MIN_SPEED = 0.0001;
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

    private bool TryGetSceneBounds( out BoundingBox bound )
    {
      bound = default;

      if ( Model is null )
        return false;

      return Model.SceneNode.TryGetBound( out bound );
    }

    #endregion

    #region Overrides

    protected override void OnGotMouseCapture( MouseEventArgs e )
    {
      if ( IsFlycamEnabled )
        _isFocusedEvent.Set();
    }

    protected override void OnLostMouseCapture( MouseEventArgs e )
    {
      if ( IsFlycamEnabled )
        _isFocusedEvent.Reset();
    }

    protected override void OnPreviewMouseLeftButtonDown( MouseButtonEventArgs e )
    {
      base.OnPreviewMouseLeftButtonDown( e );

      if ( !IsFlycamEnabled )
        return;

      Focus();
      CaptureMouse();
      Cursor = Cursors.None;
      _isMouseCaptured = true;

      var capturePoint = PointToScreen( e.GetPosition( this ) );
      _lastMousePos = new Point( ( int ) capturePoint.X, ( int ) capturePoint.Y );
    }

    protected override void OnPreviewMouseLeftButtonUp( MouseButtonEventArgs e )
    {
      base.OnPreviewMouseLeftButtonUp( e );

      if ( !IsFlycamEnabled )
        return;

      _isMouseCaptured = false;
      ReleaseMouseCapture();
      Cursor = Cursors.Arrow;
    }

    #endregion

    #region Event Handlers

    private void OnViewportPreviewKeyDown( object sender, KeyEventArgs e )
    {
      // Prevent the Viewport from using any of these input bindings.
      switch ( e.Key )
      {
        case Key.W:
        case Key.A:
        case Key.S:
        case Key.D:
        case Key.Q:
        case Key.E:
        case Key.R:
        case Key.F:
          e.Handled = true;
          break;
      }
    }

    private void OnFrameRendered( object? sender, EventArgs e )
      => _isFrameRendered = true;

    private static void OnMoveSpeedPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      var control = d as MeshView;
      if ( control is null )
        return;

      control._minMoveSpeed = control.MinMoveSpeed;
      control._moveSpeed = control.MoveSpeed;
      control._maxMoveSpeed = control.MaxMoveSpeed;
    }

    private static void OnModelPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      var control = d as MeshView;
      if ( control is null )
        return;

      var model = e.NewValue as SceneNodeGroupModel3D;
      if ( model is null )
        return;

      control.RecalculateMoveSpeed();
    }

    #endregion

    #region IDisposable Methods

    public void Dispose()
    {
      DisposeMonitorThread();
      DisposeEventHandlers();

      _moveSpeedThrottler?.Dispose();

      Viewport.RenderHost.StopRendering();
      Viewport.RenderHost.EndD3D();
      Viewport.RenderHost.Dispose();
      Viewport.Dispose();

      Viewport = null;
      Camera = null;
      Model = null;
    }

    #endregion

  }

}
