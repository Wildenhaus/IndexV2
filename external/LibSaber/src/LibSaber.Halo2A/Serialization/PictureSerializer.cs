using LibSaber.Halo2A.Enumerations;
using LibSaber.Halo2A.IO;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class PictureSerializer : H2ASerializerBase<Picture>
  {

    #region Constants

    private const int SIGNATURE_PICT = 0x50494354; // TCIP

    #endregion

    #region Overrides

    public override Picture Deserialize( NativeReader reader, ISerializationContext context )
    {
      var pict = new Picture();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case PictureSentinelIds.Header:
            ReadHeader( reader, pict );
            break;
          case PictureSentinelIds.Dimensions:
            ReadDimensions( reader, pict );
            break;
          case PictureSentinelIds.Format:
            ReadFormat( reader, pict );
            break;
          case PictureSentinelIds.MipMaps:
            ReadMipMaps( reader, pict );
            break;
          case PictureSentinelIds.Data:
            ReadData( reader, pict, sentinelReader.EndOffset );
            break;

          case PictureSentinelIds.Footer:
            return pict;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return pict;
    }

    #endregion

    #region Private Methods

    private void ReadHeader( NativeReader reader, Picture pict )
    {
      ASSERT( reader.ReadInt32() == SIGNATURE_PICT, "Invalid PICT signature." );
    }

    private void ReadDimensions( NativeReader reader, Picture pict )
    {
      pict.Width = reader.ReadInt32();
      pict.Height = reader.ReadInt32();
      pict.Depth = reader.ReadInt32();
      pict.Faces = reader.ReadInt32();
    }

    private void ReadFormat( NativeReader reader, Picture pict )
    {
      var formatValue = reader.ReadInt32();
      var format = ( PictureFormat ) formatValue;

      ASSERT( Enum.IsDefined( typeof( PictureFormat ), format ),
        $"Unknown DDS Format Value: {formatValue:X}" );

      pict.Format = format;
    }

    private void ReadMipMaps( NativeReader reader, Picture pict )
    {
      pict.MipMapCount = reader.ReadInt32();
    }

    private void ReadData( NativeReader reader, Picture pict, long endOffset )
    {
      var dataSize = endOffset - reader.BaseStream.Position;

      pict.Data = new byte[ dataSize ];
      reader.Read( pict.Data );
    }

    #endregion

    #region Picture Sentinel Ids

    private class PictureSentinelIds
    {
      public const short Header = 0x00F0;
      public const short Dimensions = 0x0102;
      public const short Format = 0x00F2;
      public const short MipMaps = 0x00F9;
      public const short Data = 0x00FF;
      public const short Footer = 0x0001;
    }

    #endregion

  }

}
