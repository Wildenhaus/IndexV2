using LibSaber.HaloCEA.Enumerations;
using LibSaber.HaloCEA.Files;

namespace LibSaber.HaloCEA.Tests.Serialization
{

  public class SerializationTestBase : TestBase
  {

    #region Data Members

    private static readonly Dictionary<string, S3DPakFile> _s3dPakFiles;

    #endregion

    #region Constructor

    static SerializationTestBase()
    {
      _s3dPakFiles = InitS3DPakFiles();
    }

    #endregion

    #region Public Methods

    public static IEnumerable<object[]> GetS3DPakFilesOfType( CEAFileType fileType )
    {
      foreach ( var pakFile in _s3dPakFiles.Values )
      {
        var matchingFiles = pakFile.Entries.Values.Where( x => x.Type == fileType );
        foreach ( var matchingFile in matchingFiles )
          yield return new object[] { matchingFile };
      }
    }

    #endregion

    #region Private Methods

    private static Dictionary<string, S3DPakFile> InitS3DPakFiles()
    {
      var files = new Dictionary<string, S3DPakFile>();

      foreach ( var fileParam in GetFilePathsWithExtension( ".s3dpak" ) )
      {
        var filePath = fileParam[ 0 ].ToString();
        var fileName = Path.GetFileNameWithoutExtension( filePath );

        var pak = S3DPakFile.FromFile( filePath );
        files.Add( fileName, pak );
      }

      return files;
    }

    #endregion

  }

}
