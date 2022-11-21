using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Extensions
{

  public class ListViewColumns : DependencyObject
  {

    #region Properties

    public static readonly DependencyProperty StretchProperty = DependencyProperty.RegisterAttached(
      "Stretch",
      typeof( bool ),
      typeof( ListViewColumns ),
      new UIPropertyMetadata( true, null, OnCoerceStretch ) );

    public static bool GetStretch( DependencyObject obj )
      => ( bool ) obj.GetValue( StretchProperty );

    public static void SetStretch( DependencyObject obj, bool value )
      => obj.SetValue( StretchProperty, value );

    #endregion

    #region Event Handlers

    private static object OnCoerceStretch( DependencyObject source, object value )
    {
      var listView = source as ListView;
      if ( listView is null )
        throw new ArgumentException( "This property may only be used on ListViews" );

      listView.Loaded += new RoutedEventHandler( OnListViewLoadedForStretch );
      listView.SizeChanged += new SizeChangedEventHandler( OnListViewSizeChangedForStretch );
      return value;
    }

    private static void OnListViewSizeChangedForStretch( object sender, SizeChangedEventArgs e )
    {
      var listView = sender as ListView;
      if ( listView.IsLoaded )
        SetColumnWidthsForStretch( listView );
    }

    private static void OnListViewLoadedForStretch( object sender, RoutedEventArgs e )
    {
      var listView = sender as ListView;
      SetColumnWidthsForStretch( listView );
    }

    private static void SetColumnWidthsForStretch( ListView listView )
    {
      var columns = listView.Tag as List<GridViewColumn>;
      double specifiedWidth = 0;
      var gridView = listView.View as GridView;
      if ( gridView != null )
      {
        if ( columns == null )
        {
          columns = new List<GridViewColumn>();
          foreach ( var column in gridView.Columns )
          {
            if ( !( column.Width >= 0 ) )
              columns.Add( column );
            else
              specifiedWidth += column.ActualWidth;
          }
        }
        else
        {
          foreach ( var column in gridView.Columns )
          {
            if ( !columns.Contains( column ) )
              specifiedWidth += column.ActualWidth;
          }
        }

        foreach ( GridViewColumn column in columns )
        {
          double newWidth = ( listView.ActualWidth - specifiedWidth ) / columns.Count;
          if ( newWidth >= 10 )
            column.Width = newWidth - 10;
        }

        listView.Tag = columns;
      }
    }

    #endregion

  }

}