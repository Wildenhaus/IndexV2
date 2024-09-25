using System.Globalization;
using System.Threading;
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
      SetApplicationCulture();
      new Startup().Run();
    }

    #endregion

    #region Private Methods

    private void SetApplicationCulture()
    {
      // Issue #11: European users may have their system/runtime default to commas
      // for denoting the decimal separator. Force set the culture to en-US so
      // that things correctly parse.

      var culture = new CultureInfo( "en-US" );

      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;

      CultureInfo.DefaultThreadCurrentCulture = culture;
      CultureInfo.DefaultThreadCurrentUICulture = culture;
    }

    #endregion

  }

}
