using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Index.UI.Converters
{

  public class VisibilityConverter : IValueConverter
  {

    public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
      => Convert( value, targetType );

    public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
      => Convert( value, targetType );

    private static object Convert( object value, Type targetType )
    {
      if ( targetType == typeof( bool ) )
        return ConvertToBool( value );

      if ( targetType == typeof( Visibility ) )
        return ConvertToVisibility( value );

      throw new ArgumentException( $"Unsupported type: {targetType.Name}" );
    }

    private static bool ConvertToBool( object value )
    {
      if ( value is Visibility visibilityValue )
        return GetBool( visibilityValue );

      return false;
    }

    private static Visibility ConvertToVisibility( object value )
    {
      if ( value is bool boolValue )
        return GetVisibility( boolValue );

      return Visibility.Collapsed;
    }

    private static Visibility GetVisibility( bool value )
      => value ? Visibility.Visible : Visibility.Collapsed;

    private static bool GetBool( Visibility visibility )
      => visibility == Visibility.Visible;

  }

}
