using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Index.UI.Extensions
{

  public sealed class DoubleClickBehavior : DependencyObject
  {

    #region Properties

    public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
      "Command",
      typeof( ICommand ),
      typeof( DoubleClickBehavior ),
      new PropertyMetadata( default( ICommand ), OnCommandChanged ) ); 

    public static void SetCommand( DependencyObject element, ICommand value )
      => element.SetValue( CommandProperty, value );

    public static ICommand GetCommand( DependencyObject element )
      => ( ICommand ) element.GetValue( CommandProperty );

    public static readonly DependencyProperty CommandParameterProperty = DependencyProperty.RegisterAttached(
      "CommandParameter",
      typeof( object ),
      typeof( DoubleClickBehavior ),
      new PropertyMetadata( default( object ) ) );

    public static void SetCommandParameter( DependencyObject element, object value )
      => element.SetValue( CommandParameterProperty, value );

    public static object GetCommandParameter( DependencyObject element )
      => ( object ) element.GetValue( CommandParameterProperty );

    #endregion

    #region Event Handlers

    private static void OnCommandChanged( DependencyObject sender, DependencyPropertyChangedEventArgs e )
    {
      var control = sender as Control;
      ASSERT_NOT_NULL( control, "DoubleClickBehavior can only be applied to controls." );

      control.MouseDoubleClick -= OnDoubleClick;
      
      if( GetCommand( control ) != null )
        control.MouseDoubleClick += OnDoubleClick;
    }

    private static void OnDoubleClick( object sender, MouseButtonEventArgs e )
    {
      var element = sender as DependencyObject;
      ASSERT_NOT_NULL( element );

      var command = GetCommand( element );
      if ( command is null)
        return;

      var parameter = GetCommandParameter( element );
      if ( !command.CanExecute( parameter ) )
        return;

      command.Execute( parameter );
    }

    #endregion

  }

}
