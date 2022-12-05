using LibSaber.HaloCEA.Files;

namespace LibSaber.HaloCEA.Tests.Files
{

  public class S3DPakFileTests : TestBase
  {

    [Theory]
    [MemberData( nameof( GetFilePathsWithExtension ), parameters: ".s3dpak" )]
    public void S3DPakFile_Initializes( string filePath )
    {
      //== Act ==================================
      var pak = S3DPakFile.FromFile( filePath );

      //== Assert ===============================
      Assert.NotEmpty( pak.FilePath );
      Assert.NotEmpty( pak.Entries );
    }

  }

}
