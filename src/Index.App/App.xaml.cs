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
      var bootstrapper = new Bootstrapper();
      bootstrapper.Run();
    }

    #endregion

  }

}
