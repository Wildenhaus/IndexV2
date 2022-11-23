using System.Windows;
using System.Windows.Input;
using Prism.Commands;

namespace Index.UI.Windows
{

  public static class WindowCommands
  {

    public static ICommand CloseWindowCommand { get; }
    public static ICommand MaximizeWindowCommand { get; }
    public static ICommand MinimizeWindowCommand { get; }

    static WindowCommands()
    {
      CloseWindowCommand = new DelegateCommand<Window>( CloseWindow );
      MaximizeWindowCommand = new DelegateCommand<Window>( MaximizeWindow );
      MinimizeWindowCommand = new DelegateCommand<Window>( MinimizeWindow );
    }

    private static void CloseWindow(Window window)
    {
      if (window is null) return;
      window.Close();
    }

    private static void MaximizeWindow(Window window)
    {
      if (window is null) return;
      window.WindowState = WindowState.Maximized;
    }

    private static void MinimizeWindow(Window window)
    {
      if (window is null) return;
      window.WindowState = WindowState.Minimized;
    }
  }

}
