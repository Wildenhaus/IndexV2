using Index.Domain;
using Index.Modules.DataExplorer.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Index.Modules.DataExplorer
{

  public class DataExplorerModule : IModule
  {

    #region Data Members

    private readonly IRegionManager _regionManager;

    #endregion

    #region Constructor

    public DataExplorerModule( IRegionManager regionManager )
    {
      _regionManager = regionManager;
    }

    #endregion

    #region Public Methods

    public void OnInitialized( IContainerProvider containerProvider )
    {
      _regionManager.RegisterViewWithRegion( RegionKeys.DataExplorerRegion, typeof( DataExplorerView ) );
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
    }

    #endregion

  }

}
