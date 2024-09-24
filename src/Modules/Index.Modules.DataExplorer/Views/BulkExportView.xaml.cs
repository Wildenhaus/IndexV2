using System;
using System.Windows;
using System.Windows.Controls;
using Index.Modules.DataExplorer.ViewModels;
using Prism.Regions;

namespace Index.Modules.DataExplorer.Views
{
  /// <summary>
  /// Interaction logic for BulkExportView.xaml
  /// </summary>
  public partial class BulkExportView : UserControl
  {
    public BulkExportView()
    {
      InitializeComponent();
      DataContextChanged += OnDataContextChanged;
    }

    protected override void OnInitialized( EventArgs e )
    {
      base.OnInitialized( e );
      var vm = DataContext as BulkExportViewModel;
      if ( vm is null )
        return;

      RegionManager.SetRegionManager( OptionsTabs, vm.RegionManager );
    }

    private void OnDataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
    {
      var vm = e.NewValue as BulkExportViewModel;
      if ( vm is null )
        return;

      RegionManager.SetRegionManager( OptionsTabs, vm.RegionManager );
    }

  }
}
