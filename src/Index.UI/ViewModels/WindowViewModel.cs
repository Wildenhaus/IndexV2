using System.ComponentModel;
using Index.UI.Windows;
using Prism.Ioc;
using Prism.Regions;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  public abstract class WindowViewModel : ViewModelBase, IWindowViewModel
  {

    #region Properties

    public string Title { get; set; }

    [DoNotNotify] protected IContainerProvider Container { get; }
    [DoNotNotify] protected IRegionManager RegionManager { get; }
    [DoNotNotify] protected IxWindow Window { get; private set; }

    #endregion

    #region Constructor

    protected WindowViewModel( IContainerProvider container )
    {
      Container = container;
      if ( container != null )
        RegionManager = container.Resolve<IRegionManager>();
    }

    #endregion

    #region Virtual Methods

    protected virtual void OnWindowAppeared()
    {
    }

    protected virtual void OnWindowClosing( CancelEventArgs e )
    {
    }

    #endregion

    #region Private Methods

    protected virtual void Close()
      => Window.Close();

    #endregion

    #region Internal Methods

    internal void SetWindow( IxWindow window )
    {
      Window = window;
      Window.Closing += ( s, e ) => OnWindowClosing( e );
    }

    internal void WindowAppeared()
      => OnWindowAppeared();

    internal void WindowClosing( CancelEventArgs e )
      => OnWindowClosing( e );

    #endregion

  }

}
