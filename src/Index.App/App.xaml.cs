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
      new Startup().Run();
    }

    #endregion

  }

}
