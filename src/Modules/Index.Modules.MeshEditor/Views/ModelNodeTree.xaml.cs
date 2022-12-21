using System.Windows;
using System.Windows.Controls;

namespace Index.Modules.MeshEditor.Views
{

  public partial class ModelNodeTree : UserControl
  {

    #region Constructor

    public ModelNodeTree()
    {
      InitializeComponent();
    }

    #endregion

    #region Private Methods

    private void UpdateColumnsWidth( ListView listView )
    {
      if ( listView is null )
        return;

      var gridView = listView.View as GridView;
      if ( gridView is null )
        return;

      var lastColumnIdx = gridView.Columns.Count - 1;
      if ( listView.ActualWidth == double.NaN )
        listView.Measure( new Size( double.PositiveInfinity, double.PositiveInfinity ) );

      var remainingSpace = listView.ActualWidth;
      for ( int i = 0; i < gridView.Columns.Count; i++ )
        if ( i != lastColumnIdx )
          remainingSpace -= gridView.Columns[ i ].ActualWidth;

      gridView.Columns[ lastColumnIdx ].Width = remainingSpace >= 0 ? remainingSpace : 0;
    }

    #endregion

    #region Event Handlers

    private void OnListViewSizeChanged( object sender, SizeChangedEventArgs e )
      => UpdateColumnsWidth( sender as ListView );

    private void OnListViewLoaded( object sender, RoutedEventArgs e )
      => UpdateColumnsWidth( sender as ListView );

    #endregion

  }

}
