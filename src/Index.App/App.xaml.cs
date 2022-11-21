using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Index.App
{

  public partial class App : Application
  {

    #region Data Members



    #endregion

    #region Event Handlers

    private void OnApplicationStartup( object sender, StartupEventArgs e )
    {
#if DEBUG && false
      DebugLaunch( e.Args );
#else
      DefaultLaunch();
#endif
    }

    #endregion

    #region Private Methods

    private void DefaultLaunch()
    {
      var bootstrapper = new Bootstrapper();
      bootstrapper.Run();
    }

    private void DebugLaunch( string[] args )
    {
      if ( args.Length < 2 )
      {
        DefaultLaunch();
        return;
      }

      var gameId = args[ 0 ];
      var gamePath = args[ 1 ];
      var bootstrapper = new DebugBootstrapper( gameId, gamePath );
      bootstrapper.Run();
    }

    #endregion

  }

}
