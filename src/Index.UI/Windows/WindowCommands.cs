using System.Windows;

namespace Index.UI.Windows
{

  public class CloseWindowCommand : CommandBase
  {
    public override void Execute( object? parameter )
    {
      if ( parameter is Window window )
        window.Close();
    }
  }

  public class MaximizeWindowCommand : CommandBase
  {
    public override void Execute( object? parameter )
    {
      if ( parameter is Window window )
      {
        if ( window.WindowState == WindowState.Maximized )
          window.WindowState = WindowState.Normal;
        else
          window.WindowState = WindowState.Maximized;
      }
    }
  }

  public class MinimizeWindowCommand : CommandBase
  {
    public override void Execute( object? parameter )
    {
      if ( parameter is Window window )
        window.WindowState = WindowState.Minimized;
    }
  }

}
