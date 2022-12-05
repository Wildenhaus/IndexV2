using LibSaber.HaloCEA.Enumerations;
using LibSaber.HaloCEA.IO;
using LibSaber.IO;

namespace LibSaber.HaloCEA.Files
{

  public class IPakFile : CEAContainerFileBase<IPakFile, IPakFileEntry>
  {

    #region Constants

    private const int TEXTURE_NAME_LENGTH = 0x100;
    private const long TEXTURE_DATA_START_OFFSET = 0x290008;

    #endregion

    #region Constructor

    private IPakFile( string filePath, CEAStreamCompressionInfo compressionInfo = default )
      : base( filePath, compressionInfo )
    {
    }

    public static IPakFile FromFile( string filePath )
    {
      var pak = new IPakFile( filePath );
      pak.Initialize();

      return pak;
    }

    #endregion

    #region Private Methods

    private void Initialize()
    {
      var reader = new NativeReader( GetStream(), Endianness.LittleEndian );

      var entryCount = reader.ReadInt32();
      var unk_00 = reader.ReadInt32();

      for ( var i = 0; i < entryCount; i++ )
      {
        var entry = ReadEntry( reader );
        AddEntry( entry );
      }
    }

    private IPakFileEntry ReadEntry( NativeReader reader )
    {
      var entry = new IPakFileEntry( this );

      // The texture's name is always 0x100 bytes long.
      entry.Name = reader.ReadFixedLengthString( TEXTURE_NAME_LENGTH );
      _ = reader.ReadInt64();
      _ = reader.ReadInt32();

      entry.Width = reader.ReadInt32();
      entry.Height = reader.ReadInt32();
      entry.Depth = reader.ReadInt32();

      entry.MipCount = reader.ReadInt32();
      entry.FaceCount = reader.ReadInt32();
      entry.Format = ( CEATextureFormat ) reader.ReadInt32();
      _ = reader.ReadInt64();

      // This part is weird.
      // The size of the raw texture data is stored three times.
      // We're reading all of them so we can assert their equality.

      entry.SizeInBytes = reader.ReadInt32();
      _ = reader.ReadInt32();

      var size_B = reader.ReadInt32();
      entry.StartOffset = reader.ReadInt32();
      _ = reader.ReadInt32();

      var size_C = reader.ReadInt32();
      _ = reader.ReadInt32();

      ASSERT( entry.SizeInBytes == size_B, "Texture size mismatch." );
      ASSERT( entry.SizeInBytes == size_C, "Texture size mismatch." );
      ASSERT( entry.StartOffset >= TEXTURE_DATA_START_OFFSET,
        "Texture data start offset is out of bounds." );

      return entry;
    }

    #endregion

  }

}
