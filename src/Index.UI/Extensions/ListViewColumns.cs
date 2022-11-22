using Index.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Extensions
{

  public class ListViewColumns : DependencyObject
  {

    #region Properties

    public static readonly DependencyProperty StretchProperty = DependencyProperty.RegisterAttached(
      "Stretch",
      typeof( ListViewColumnStretchType ),
      typeof( ListViewColumns ),
      new UIPropertyMetadata( ListViewColumnStretchType.None, null, OnCoerceStretch ) );

    public static ListViewColumnStretchType GetStretch( DependencyObject obj )
      => ( ListViewColumnStretchType ) obj.GetValue( StretchProperty );

    public static void SetStretch( DependencyObject obj, ListViewColumnStretchType value )
      => obj.SetValue( StretchProperty, value );

    #endregion

    #region Event Handlers

    private static object OnCoerceStretch( DependencyObject source, object value )
    {
      var listView = source as ListView;
      ASSERT_NOT_NULL( listView );

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
      var stretchType = GetStretch( listView );
      if ( stretchType == ListViewColumnStretchType.None )
        return;

      CalculateStretchStatistics( listView, out var stretchableColumns, out var remainingWidth );
      if ( stretchableColumns.Count == 0 )
        return;

      var columnsToStretch = stretchableColumns.ToArray();
      switch ( stretchType )
      {
        case ListViewColumnStretchType.AllColumns:
          StretchColumns( columnsToStretch, remainingWidth );
          break;
        case ListViewColumnStretchType.LastColumn:
          StretchColumns( columnsToStretch[ ^1.. ], remainingWidth );
          break;
      }

    }

    private static void StretchColumns( Span<GridViewColumn> columns, double remainingWidth )
    {
      var allottedWidth = remainingWidth / columns.Length;
      if ( allottedWidth < 1 )
        return;

      if ( allottedWidth >= 10 )
        allottedWidth -= 10;

      foreach ( var column in columns )
      {
        if ( column.Width > 0 )
          continue;

        column.Width = column.ActualWidth + allottedWidth;
      }
    }

    private static void CalculateStretchStatistics( ListView listView,
      out List<GridViewColumn> stretchableColumns,
      out double remainingWidth )
    {
      stretchableColumns = new List<GridViewColumn>();
      remainingWidth = listView.ActualWidth;

      var gridView = listView.View as GridView;
      if ( gridView is null )
        return;

      foreach ( var column in gridView.Columns )
      {
        if ( double.IsNaN( column.Width ) )
          stretchableColumns.Add( column );

        remainingWidth -= column.ActualWidth;
      }
    }

    #endregion

    #region Embedded Types

    public enum ListViewColumnStretchType
    {
      None,
      AllColumns,
      LastColumn
    }

    #endregion

  }

}