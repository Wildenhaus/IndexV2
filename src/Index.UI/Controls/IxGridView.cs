using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Controls
{

  public class IxGridView : GridView
  {

    #region Properties

    public static DependencyProperty AutoSizeColumnWidthProperty = DependencyProperty.Register(
     nameof( AutoSizeColumnWidth ),
     typeof( bool ),
     typeof( IxGridView ) );

    public bool AutoSizeColumnWidth
    {
      get => ( bool ) GetValue( AutoSizeColumnWidthProperty );
      set => SetValue( AutoSizeColumnWidthProperty, value );
    }

    #endregion

    #region Overrides

    protected override void PrepareItem( ListViewItem item )
    {
      if ( AutoSizeColumnWidth )
        foreach ( var column in Columns )
          PerformAutoSizeColumnWidth( column );

      base.PrepareItem( item );
    }

    #endregion

    #region Private Methods

    private void PerformAutoSizeColumnWidth( GridViewColumn column )
    {
      if ( double.IsNaN( column.Width ) )
        column.Width = column.ActualWidth;

      column.Width = double.NaN;
    }

    #endregion

  }

}
