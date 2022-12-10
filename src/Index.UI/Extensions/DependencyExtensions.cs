using System.Windows;
using System.Windows.Media;

namespace Index.UI.Extensions
{

  public static class DependencyExtensions
  {

    public static T FindParent<T>( this DependencyObject child )
      where T : DependencyObject
    {
      var parentObject = VisualTreeHelper.GetParent( child );
      if ( parentObject is null )
        return null;

      var parent = parentObject as T;
      if ( parent is not null )
        return parent;

      return FindParent<T>( parentObject );
    }

  }

}
