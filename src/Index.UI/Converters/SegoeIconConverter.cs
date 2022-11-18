using System;
using System.Globalization;
using System.Windows.Data;

namespace Index.UI.Converters
{

  public class SegoeIconConverter : IValueConverter
  {

    public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
    {
      if ( targetType == typeof( string ) )
        return ( ( SegoeIcon ) value ).AsString();
      if ( targetType == typeof( char ) )
        return ( ( SegoeIcon ) value ).AsChar();

      throw new NotSupportedException();
    }

    public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
    {
      if ( value is string && targetType == typeof( SegoeIcon ) )
        return ( ( ( string ) value )[ 0 ] ).AsSegoeIcon();
      if ( value is char && targetType == typeof( SegoeIcon ) )
        return ( ( ( char ) value ) ).AsSegoeIcon();

      throw new NotSupportedException();
    }

  }

}
