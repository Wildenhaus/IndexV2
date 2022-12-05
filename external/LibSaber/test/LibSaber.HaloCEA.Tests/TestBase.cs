using System.Text.RegularExpressions;

namespace LibSaber.HaloCEA.Tests
{

  public abstract class TestBase
  {

    public static IEnumerable<object[]> GetFilePathsWithExtension( string extension )
    {
      // Sanitize extension
      extension = Regex.Replace( extension, "[*.]", "" );
      extension = "*." + extension;

      var files = Directory.EnumerateFiles( TestConfig.CEA_INSTALL_PATH, extension, SearchOption.AllDirectories );
      foreach ( var file in files )
        yield return new object[] { file };
    }

  }

}
