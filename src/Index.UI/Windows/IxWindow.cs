using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Shapes;
using Index.UI.Common;
using Index.UI.ViewModels;

namespace Index.UI.Windows
{

  public class IxWindow : Window
  {

    #region Properties

    public bool CanClose { get; set; } = true;
    public Path? TitleBarIcon { get; set; }

    #endregion

    #region Constructor

    public IxWindow()
    {
      ContentRendered += OnContentRendered;
      DataContextChanged += OnDataContextChanged;
      SourceInitialized += OnSourceInitialized;
    }

    static IxWindow()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxWindow ),
        new FrameworkPropertyMetadata( typeof( IxWindow ) ) );
    }

    #endregion

    #region Private Methods

    private IntPtr WindowProc( IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled )
    {
      switch ( msg )
      {
        case 0x0024:
          Win32.WmGetMinMaxInfo( hwnd, lParam, ( int ) MinWidth, ( int ) MinHeight );
          handled = true;
          break;
      }

      return IntPtr.Zero;
    }

    #endregion

    #region Event Handlers

    private void OnContentRendered( object? sender, System.EventArgs e )
    {
      var viewModel = DataContext as WindowViewModel;
      if ( viewModel is not null )
        viewModel.WindowAppeared();

      ContentRendered -= OnContentRendered;
    }

    private void OnDataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
    {
      var viewModel = e.NewValue as WindowViewModel;
      if ( viewModel is null )
        return;

      viewModel.SetWindow( this );
    }

    private void OnSourceInitialized( object? sender, EventArgs e )
    {
      var handle = new WindowInteropHelper( this ).Handle;
      HwndSource.FromHwnd( handle )?.AddHook( WindowProc );
    }

    #endregion

  }

}
