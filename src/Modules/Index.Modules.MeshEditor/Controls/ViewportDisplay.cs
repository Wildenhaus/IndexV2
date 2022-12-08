using System.Windows;
using System.Windows.Data;
using HelixToolkit.Wpf.SharpDX.Elements2D;
using SharpDX.Direct2D1;
using Orientation = System.Windows.Controls.Orientation;

namespace Index.Modules.MeshEditor.Controls
{

  public class ViewportDisplay : StackPanel2D
  {

    #region Properties

    public static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
      nameof( Foreground ),
      typeof( System.Windows.Media.Brush ),
      typeof( ViewportDisplay ) );

    public Brush Foreground
    {
      get => ( Brush ) GetValue( ForegroundProperty );
      set => SetValue( ForegroundProperty, value );
    }

    public static readonly DependencyProperty FramesPerSecondProperty = DependencyProperty.Register(
      nameof( FramesPerSecond ),
      typeof( double ),
      typeof( ViewportDisplay ) );

    public double FramesPerSecond
    {
      get => ( double ) GetValue( FramesPerSecondProperty );
      set => SetValue( FramesPerSecondProperty, value );
    }

    #endregion

    #region Constructor

    public ViewportDisplay()
    {
      Orientation = Orientation.Vertical;
      InitializeElements();
    }

    #endregion

    private void InitializeElements()
    {
      AddBoundChild( FramesPerSecondProperty, "FPS: ", "{0:0.0}" );
    }

    private void AddBoundChild( DependencyProperty boundProperty, string label = null, string formatString = null )
    {
      var childPanel = new StackPanel2D
      {
        Orientation = Orientation.Horizontal,
        Margin = new Thickness( 2 )
      };

      if ( !string.IsNullOrWhiteSpace( label ) )
      {
        var labelModel = CreateTextModel2D();
        labelModel.Text = label;
        childPanel.Children.Add( labelModel );
      }

      var contentModel = CreateTextModel2D();
      BindProperty( contentModel, TextModel2D.TextProperty, this, boundProperty, formatString );
      childPanel.Children.Add( contentModel );

      Children.Add( childPanel );
    }

    private TextModel2D CreateTextModel2D()
    {
      var model = new TextModel2D
      {
        HorizontalAlignment = HorizontalAlignment.Left,
        VerticalAlignment = VerticalAlignment.Center,
      };
      BindProperty( model, TextModel2D.ForegroundProperty, this, ForegroundProperty );

      return model;
    }

    private void BindProperty(
      FrameworkContentElement destObject, DependencyProperty destProperty,
      DependencyObject sourceObject, DependencyProperty source, string stringFormat = null )
    {
      var binding = new Binding( source.Name );
      binding.Source = sourceObject;
      if ( !string.IsNullOrWhiteSpace( stringFormat ) )
        binding.StringFormat = stringFormat;

      destObject.SetBinding( destProperty, binding );
    }

  }

}
