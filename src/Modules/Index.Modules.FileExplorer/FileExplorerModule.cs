using Index.Modules.FileExplorer.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace Index.Modules.FileExplorer
{

  public class FileExplorerModule : IModule
  {

    private IRegionManager _regionManager;

    public FileExplorerModule( IRegionManager regionManager )
    {
      _regionManager = regionManager;
    }

    public void OnInitialized( IContainerProvider containerProvider )
    {
      _regionManager.RegisterViewWithRegion( "FileExplorerContent", typeof( FileExplorerView ) );
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
    }

  }

}
