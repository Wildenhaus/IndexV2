using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Index.UI.Controls
{

  public class IxTextBox : TextBox
  {

    #region Data Members

    private bool _isSubmitting;

    #endregion

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

    public static readonly DependencyProperty SubmitTextCommandProperty = DependencyProperty.Register(
      nameof( SubmitTextCommand ),
      typeof( ICommand ),
      typeof( IxTextBox ) );

    public ICommand SubmitTextCommand
    {
      get => ( ICommand ) GetValue( SubmitTextCommandProperty );
      set => SetValue( SubmitTextCommandProperty, value );
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

    protected override void OnPreviewKeyDown( KeyEventArgs e )
    {
      base.OnPreviewKeyDown( e );

      if ( e.Key == Key.Enter )
        SubmitText();
    }

    #endregion

    #region Private Methods

    private void SubmitText()
    {
      var submitCommand = SubmitTextCommand;
      if ( _isSubmitting || submitCommand is null )
        return;

      IsEnabled = false;
      _isSubmitting = true;
      var text = Text;

      Task.Run( () =>
      {
        submitCommand.Execute( text );
      } )
        .ContinueWith( t =>
        {
          _isSubmitting = false;
          Dispatcher.Invoke( () => { IsEnabled = true; } );
        } );
    }

    #endregion

  }

}
