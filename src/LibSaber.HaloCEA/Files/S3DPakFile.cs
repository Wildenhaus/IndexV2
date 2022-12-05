using System.Xml.Linq;
using LibSaber.HaloCEA.Enumerations;
using LibSaber.HaloCEA.IO;
using LibSaber.IO;

namespace LibSaber.HaloCEA.Files
{

  public class S3DPakFile : CEAContainerFileBase<S3DPakFile, S3DPakFileEntry>
  {

    #region Constructor

    private S3DPakFile( string filePath, CEAStreamCompressionInfo compressionInfo = default )
      : base( filePath )
    {
    }

    public static S3DPakFile FromFile( string filePath )
    {
      var pak = new S3DPakFile( filePath );
      pak.Initialize();

      return pak;
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
      var reader = new NativeReader( GetStream(), Endianness.LittleEndian );

      var entryCount = reader.ReadInt32();
      for ( var i = 0; i < entryCount; i++ )
      {
        var entry = ReadEntry( reader );
        AddEntry( entry );
      }
    }

    private S3DPakFileEntry ReadEntry( NativeReader reader )
    {
      var entry = new S3DPakFileEntry( this );
      entry.StartOffset = reader.ReadInt32();
      entry.SizeInBytes = reader.ReadInt32();
      entry.Name = reader.ReadLengthPrefixedString32();
      entry.Type = ( CEAFileType ) reader.ReadInt32();
      var unk_00 = reader.ReadInt64();

      // Some files don't have names. These are unique to their type.
      if ( string.IsNullOrWhiteSpace( entry.Name ) )
        entry.Name = entry.Type.ToString();

      return entry;
    }

    #endregion

  }

}
