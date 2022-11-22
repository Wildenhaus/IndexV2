using Index.App.ViewModels;
using Index.UI.Windows;

namespace Index.App.Views
{

  public partial class LauncherView : IxDialogWindow
  {

    public LauncherView( LauncherViewModel viewModel )
    {
      DataContext = viewModel;
      InitializeComponent();
    }

  }

}
