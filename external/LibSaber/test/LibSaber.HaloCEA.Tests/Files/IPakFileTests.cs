using LibSaber.HaloCEA.Files;

namespace LibSaber.HaloCEA.Tests.Files
{

  public class IPakFileTests : TestBase
  {

    [Theory]
    [MemberData( nameof( GetFilePathsWithExtension ), parameters: ".ipak" )]
    public void IPakFile_Initializes( string filePath )
    {
      //== Act ==================================
      var pak = IPakFile.FromFile( filePath );

      //== Assert ===============================
      Assert.NotEmpty( pak.FilePath );
      Assert.NotEmpty( pak.Entries );
    }

  }

}
