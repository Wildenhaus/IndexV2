using LibSaber.HaloCEA.Structures;
using LibSaber.HaloCEA.Tests;
using LibSaber.IO;
using LibSaber.Serialization;

internal class Program
{
  private static void Main( string[] args )
  {
    //var t = new TemplateSerializationTests();
    //t.TestTemplateSerialization( "G:\\h1a\\x\\a10\\assault_rifle.Template" );

    var reader = new NativeReader( File.OpenRead( "G:\\h1a\\x\\a10\\a10.lg.Scene" ), Endianness.LittleEndian );
    SaberScene.Deserialize( reader, new SerializationContext() );
  }
}