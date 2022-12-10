using System.Linq;
using Index.Modules.Logging.Logging;
using Index.Modules.Logging.Views;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Serilog;

namespace Index.Modules.Logging
{

  public class LoggingModule : IModule
  {

    #region Data Members

    private readonly IEventAggregator _eventAggregator;
    private readonly IRegionManager _regionManager;

    #endregion

    #region Constructor

    public LoggingModule( IEventAggregator eventAggregator, IRegionManager regionManager )
    {
      _eventAggregator = eventAggregator;
      _regionManager = regionManager;
    }

    #endregion

    public void OnInitialized( IContainerProvider containerProvider )
    {
      ConfigureLogger();
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<LogView>();
    }

    private void ConfigureLogger()
    {
      Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.PrismEvent( _eventAggregator )
        .WriteTo.Debug()
        .CreateLogger();
    }

  }

}
