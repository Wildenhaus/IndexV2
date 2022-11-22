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

    #endregion

    #region Constructor

    protected WindowViewModel( IContainerProvider container )
    {
      Container = container;
      RegionManager = container.Resolve<IRegionManager>();
    }

    #endregion

    #region Private Methods

    protected virtual void OnWindowAppeared()
    {
    }

    #endregion

    #region Internal Methods

    internal void WindowAppeared()
      => OnWindowAppeared();

    #endregion

  }

}
