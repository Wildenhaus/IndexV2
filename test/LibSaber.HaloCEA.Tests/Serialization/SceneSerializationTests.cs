using LibSaber.HaloCEA.Enumerations;
using LibSaber.HaloCEA.Files;
using LibSaber.HaloCEA.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.HaloCEA.Tests.Serialization
{

  public class SceneSerializationTests : SerializationTestBase
  {

    [Theory]
    [MemberData( nameof( GetS3DPakFilesOfType ), parameters: CEAFileType.Scene )]
    public void TestSceneSerialization( S3DPakFileEntry file )
    {
      //== Arrange ==============================
      var stream = file.GetStream();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      //== Act ==================================
      var template = SaberScene.Deserialize( reader, new SerializationContext() );

      //== Assert ===============================
      Assert.Equal( reader.Position, stream.Length );
    }

  }

}
