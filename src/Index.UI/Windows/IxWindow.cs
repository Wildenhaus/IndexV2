using System.ComponentModel;
using System.Windows;
using System.Windows.Shapes;
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
    }

    static IxWindow()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxWindow ),
        new FrameworkPropertyMetadata( typeof( IxWindow ) ) );
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

    #endregion

  }

}
