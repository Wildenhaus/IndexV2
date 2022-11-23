using System.Windows;
using DryIoc;

namespace Index.App
{

  public partial class App : Application
  {

    #region Properties

    public IContainer Container { get; internal set; }

    #endregion

    #region Event Handlers

    private void OnApplicationStartup( object sender, StartupEventArgs e )
    {
      new Startup().Run();
    }

    #endregion

  }

}
