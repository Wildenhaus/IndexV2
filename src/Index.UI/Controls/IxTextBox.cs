using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Controls
{

  public class IxTextBox : TextBox
  {

    #region Properties

    public static readonly DependencyProperty HasTextProperty = DependencyProperty.Register(
      nameof( HasText ),
      typeof( bool ),
      typeof( IxTextBox ) );

    public bool HasText
    {
      get => ( bool ) GetValue( HasTextProperty );
      set => SetValue( HasTextProperty, value );
    }

    public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
      nameof( Icon ),
      typeof( object ),
      typeof( IxTextBox ) );

    public object Icon
    {
      get => ( object ) GetValue( IconProperty );
      set => SetValue( IconProperty, value );
    }

    public static readonly DependencyProperty PlaceholderTextProperty = DependencyProperty.Register(
      nameof( PlaceholderText ),
      typeof( string ),
      typeof( IxTextBox ) );

    public string PlaceholderText
    {
      get => ( string ) GetValue( PlaceholderTextProperty );
      set => SetValue( PlaceholderTextProperty, value );
    }

    #endregion

    #region Constructor

    static IxTextBox()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxTextBox ),
        new FrameworkPropertyMetadata( typeof( IxTextBox ) ) );
    }

    #endregion

    #region Overrides

    protected override void OnTextChanged( TextChangedEventArgs e )
    {
      base.OnTextChanged( e );
      HasText = Text.Length != 0;
    }

    #endregion

  }

}
