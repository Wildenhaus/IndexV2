using System.Reflection;

namespace Index.Common
{

  public static class AssemblyHelpers
  {

    public static string GetVersionNumber(Assembly assembly)
    {
      return assembly.GetName().Version.ToString();
    }

    public static string? GetBuildDate(Assembly assembly)
    {
      DateTime? buildDate = null;

      var metadataAttributes = assembly.GetCustomAttributes<AssemblyMetadataAttribute>();

      // Look for the attribute with the key "BuildDate"
      foreach ( var metadata in metadataAttributes )
        if ( metadata.Key == "BuildDate" )
          buildDate = DateTime.Parse( metadata.Value );

      if ( !buildDate.HasValue )
        return "00000000-0000";

      return buildDate?.ToString("yyyyMMddhhmm");
    }

    public static string GetBuildString( Assembly assembly )
    {
      var versionNumber = GetVersionNumber(assembly);
      var buildDate = GetBuildDate(assembly);

      return string.Format( "v{0}-{1}", versionNumber, buildDate );
    }

  }

}
