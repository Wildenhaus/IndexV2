using Index.App.ViewModels;
using Index.UI.Windows;

namespace Index.App.Views
{

  public partial class LauncherView : IxWindow
  {

    public LauncherView( LauncherViewModel viewModel )
    {
      DataContext = viewModel;
      InitializeComponent();
    }

  }

}
