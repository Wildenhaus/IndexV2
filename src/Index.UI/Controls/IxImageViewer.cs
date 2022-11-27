using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Index.UI.Controls
{

  // Based on https://stackoverflow.com/a/6782715/3808312
  public class IxImageViewer : Border
  {

    #region Data Members

    private UIElement _child;

    private Point _origin;
    private Point _start;

    #endregion

    #region Properties

    public override UIElement Child
    {
      get => base.Child;
      set
      {
        if ( value is not null && value != _child )
          Initialize( value );
        base.Child = value;
      }
    }

    #endregion

    #region Public Methods

    public void Initialize( UIElement element )
    {
      _child = element;
      if ( _child is null )
        return;

      var group = new TransformGroup();
      group.Children.Add( new ScaleTransform() );
      group.Children.Add( new TranslateTransform() );

      _child.RenderTransform = group;
      _child.RenderTransformOrigin = new Point( 0, 0 );

      MouseWheel += OnMouseWheel;
      MouseLeftButtonDown += OnMouseLeftButtonDown;
      MouseLeftButtonUp += OnMouseLeftButtonUp;
      MouseMove += OnMouseMove;
      PreviewMouseRightButtonDown += OnPreviewMouseRightButtonDown;
    }

    public void Reset()
    {
      var scale = GetTransform<ScaleTransform>( _child );
      if ( scale != null )
      {
        scale.ScaleX = 1;
        scale.ScaleY = 1;
      }

      var translate = GetTransform<TranslateTransform>( _child );
      if ( translate != null )
      {
        translate.X = 0;
        translate.Y = 0;
      }
    }

    #endregion

    #region Private Methods

    private TTransform? GetTransform<TTransform>( UIElement element )
      where TTransform : Transform
    {
      var transformGroup = element.RenderTransform as TransformGroup;
      if ( transformGroup is null )
        return null;

      return transformGroup.Children.OfType<TTransform>().FirstOrDefault();
    }

    #endregion

    #region Event Handlers

    private void OnMouseWheel( object sender, System.Windows.Input.MouseWheelEventArgs e )
    {
      if ( _child is null )
        return;

      var scale = GetTransform<ScaleTransform>( _child );
      var translate = GetTransform<TranslateTransform>( _child );
      if ( scale is null || translate is null )
        return;

      var zoom = e.Delta > 0 ? 0.2 : -0.2;
      if ( !( e.Delta > 0 ) && ( scale.ScaleX < 0.4 || scale.ScaleY < 0.4 ) )
        return;

      var relative = e.GetPosition( _child );
      var absoluteX = relative.X * scale.ScaleX + translate.X;
      var absoluteY = relative.Y * scale.ScaleY + translate.Y;

      scale.ScaleX += zoom;
      scale.ScaleY += zoom;

      translate.X = absoluteX - relative.X * scale.ScaleX;
      translate.Y = absoluteY - relative.Y * scale.ScaleY;
    }

    private void OnMouseLeftButtonDown( object sender, System.Windows.Input.MouseButtonEventArgs e )
    {
      if ( _child is null )
        return;

      var translate = GetTransform<TranslateTransform>( _child );
      _start = e.GetPosition( this );
      _origin = new Point( translate.X, translate.Y );
      Cursor = Cursors.Hand;
      _child.CaptureMouse();
    }

    private void OnMouseLeftButtonUp( object sender, System.Windows.Input.MouseButtonEventArgs e )
    {
      if ( _child is null )
        return;

      _child.ReleaseMouseCapture();
      Cursor = Cursors.Arrow;
    }

    private void OnPreviewMouseRightButtonDown( object sender, System.Windows.Input.MouseButtonEventArgs e )
    {
    }

    private void OnMouseMove( object sender, System.Windows.Input.MouseEventArgs e )
    {
      if ( _child is null || !_child.IsMouseCaptured )
        return;

      var translate = GetTransform<TranslateTransform>( _child );
      var v = _start - e.GetPosition( this );
      translate.X = _origin.X - v.X;
      translate.Y = _origin.Y - v.Y;
    }

    #endregion

  }

}
