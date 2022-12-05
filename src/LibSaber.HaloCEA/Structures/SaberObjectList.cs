using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.HaloCEA.Structures
{

  public class SaberObjectList : List<SaberObject>
  {

    #region Serialization

    public static SaberObjectList Deserialize( NativeReader reader, ISerializationContext context )
    {
      var objectList = new SaberObjectList();

      var sentinelReader = new SentinelReader( reader );
      sentinelReader.Next();
      if ( sentinelReader.SentinelId == SentinelIds.Sentinel_012C )
        ReadObjectList( objectList, reader, context );
      else
      {
        reader.Position -= 6;
        objectList.Add( SaberObject.Deserialize( reader, context ) );
        reader.Position -= 6;
        return objectList;
      }

      return objectList;
    }

    private static void ReadObjectList( SaberObjectList objectList, NativeReader reader, ISerializationContext context )
    {
      var objectCount = reader.ReadInt32();

      var sentinelReader = new SentinelReader( reader );
      for ( var i = 0; i < objectCount; i++ )
      {
        sentinelReader.Next( boundsCheck: false );
        switch ( sentinelReader.SentinelId )
        {
          case 0x00F0:
            objectList.Add( SaberObject.Deserialize( reader, context ) );
            break;

          case SentinelIds.Delimiter:
            continue;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }
    }

    #endregion

  }

}
