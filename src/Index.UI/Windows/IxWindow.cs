using Index.UI.ViewModels;
using System.Windows;
using System.Windows.Shapes;

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
      //IsVisibleChanged += OnVisibilityChanged;
    }


    static IxWindow()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxWindow ),
        new FrameworkPropertyMetadata( typeof( IxWindow ) ) );
    }

    #endregion

    #region Overrides

    private void OnContentRendered( object? sender, System.EventArgs e )
    {
      var viewModel = DataContext as WindowViewModel;
      if ( viewModel != null )
        viewModel.WindowAppeared();

      ContentRendered -= OnContentRendered;
    }


    private void OnVisibilityChanged( object sender, DependencyPropertyChangedEventArgs e )
    {
      var viewModel = DataContext as WindowViewModel;
      if ( viewModel != null )
        viewModel.WindowAppeared();

      IsVisibleChanged -= OnVisibilityChanged;
    }


    #endregion


  }

}
