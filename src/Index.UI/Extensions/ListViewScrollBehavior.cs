using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Extensions
{

  public class ListViewScrollBehavior : DependencyObject
  {

    public static readonly DependencyProperty AutoScrollProperty = DependencyProperty.RegisterAttached(
      "AutoScroll",
      typeof( bool ),
      typeof( ListViewScrollBehavior ),
      new FrameworkPropertyMetadata( false,
        FrameworkPropertyMetadataOptions.AffectsArrange |
        FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
        OnAutoScrollPropertyChanged ) );

    public static bool GetAutoScroll( DependencyObject obj )
      => ( bool ) obj.GetValue( AutoScrollProperty );

    public static void SetAutoScroll( DependencyObject obj, bool value )
      => obj.SetValue( AutoScrollProperty, value );

    private static void OnAutoScrollPropertyChanged( DependencyObject d, DependencyPropertyChangedEventArgs e )
    {
      var listView = d as ListView;
      var isEnabled = ( bool ) e.NewValue;
      var collection = listView.Items.SourceCollection as INotifyCollectionChanged;

      if ( collection is null )
        return;

      void OnCollectionChanged( object sender, NotifyCollectionChangedEventArgs e )
      {
        if ( e.Action == NotifyCollectionChangedAction.Add )
        {
          var newestItem = e.NewItems[ e.NewItems.Count - 1 ];
          listView.ScrollIntoView( newestItem );
        }
      }

      if ( isEnabled )
        collection.CollectionChanged += OnCollectionChanged;
      else
        collection.CollectionChanged -= OnCollectionChanged;
    }

  }

}
