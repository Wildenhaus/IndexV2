using System.Windows;
using Index.UI.Windows;
using Prism.Ioc;
using Prism.Regions;

namespace Index.UI.ViewModels
{

  public abstract class WindowViewModel : ViewModelBase
  {

    #region Properties

    public string WindowTitle { get; set; }

    protected IContainerProvider Container { get; }
    protected IRegionManager RegionManager { get; }

    protected IxWindow Window { get; private set; }

    #endregion

    #region Constructor

    protected WindowViewModel( IContainerProvider container )
    {
      Container = container;
      RegionManager = container.Resolve<IRegionManager>();
    }

    #endregion

    #region Virtual Methods

    protected virtual void OnWindowAppeared()
    {
    }

    protected virtual void OnWindowClosing()
    {
    }

    #endregion

    #region Private Methods

    protected void Close()
    {
      OnWindowClosing();
      Window.Close();
    }

    #endregion

    #region Internal Methods

    internal void SetWindow( IxWindow window )
      => Window = window;

    internal void WindowAppeared()
      => OnWindowAppeared();

    #endregion

  }

}
