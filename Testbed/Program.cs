using System.Xml.Linq;
using Index.Domain.FileSystem;
using Index.Profiles.Halo2A.FileSystem;
using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;
using LibSaber.Halo2A.Structures.Textures;
using LibSaber.IO;

namespace Testbed
{
  internal class Program
  {

    static IFileSystem FileSystem = new FileSystem();

    static void Main( string[] args )
    {
      const string GAMEPATH = @"D:\SteamLibrary\steamapps\common\Halo The Master Chief Collection\halo2\";
      var loader = new H2AFileSystemLoader();
      loader.SetBasePath( GAMEPATH );
      FileSystem.LoadDevices(loader).Wait();

      DeserializeTds();
    }

    //private static void DeserializeTpls()
    //{
    //  foreach ( var file in FileSystem.EnumerateFiles() )
    //  {
    //    if ( Path.GetExtension( file.Name ) == ".tpl" )
    //      DeserializeTpl( file );
    //  }
    //}

    //private static void DeserializeTpl( IFileSystemNode file )
    //{
    //  var serializer = new TemplateSerializer();
    //  var ctx = new SerializationContext();

    //  Console.Write( "Deserializing {0}...", file.Name );
    //  try
    //  {
    //    serializer.Deserialize( new LibSaber.IO.NativeReader( file.Open(), LibSaber.IO.Endianness.LittleEndian ), ctx );
    //    Console.ForegroundColor = ConsoleColor.Green;
    //    Console.WriteLine( "SUCCESS" );
    //    Console.ForegroundColor = ConsoleColor.White;
    //  }
    //  catch( Exception ex )
    //  {
    //    Console.ForegroundColor = ConsoleColor.Red;
    //    Console.WriteLine( "FAILED" );
    //    Console.ForegroundColor = ConsoleColor.White;
    //  }
    //}

    private static void DeserializeTds()
    {
      foreach ( var file in FileSystem.EnumerateFiles() )
      {
        if ( Path.GetExtension( file.Name ) == ".td" )
          DeserializeTd( file );
      }
    }

    private static void DeserializeTd( IFileSystemNode file )
    {
      Console.Write( "Deserializing {0}...", file.Name );
      try
      {
        var serializer = new FileScriptingSerializer<TextureDefinition>();
        //var reader = new NativeReader(file.Open(), Endianness.LittleEndian );
        serializer.Deserialize( file.Open() );
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine( "SUCCESS" );
        Console.ForegroundColor = ConsoleColor.White;
      }
      catch ( Exception ex )
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine( "FAILED" );
        Console.ForegroundColor = ConsoleColor.White;

        var fname = Path.GetFileName( file.Name );
        using var ws = File.Create( Path.Combine(@"E:\", fname) );
        file.Open().CopyTo( ws );
        ws.Flush();
        ws.Close();
      }
    }
  }
}
