using System.Reflection;
using Index.UI.ViewModels;
using Prism.Ioc;

namespace Index.App.ViewModels
{

  public class AboutViewModel : DialogWindowViewModel
  {

    public string VersionString
    {
      get
      {
        var version = GetBuildVersion();
        return $"v{version}";
      }
    }

    public AboutViewModel( IContainerProvider container )
      : base( container )
    {
      Title = "About";
    }

    private static string GetBuildVersion()
    {
      var assembly = Assembly.GetExecutingAssembly();
      var attr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
      return attr?.InformationalVersion;
    }

  }

}
