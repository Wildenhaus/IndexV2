using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_021F )]
  public class Data_021F
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_0220 )]
    public Data_0220 Data_0220;

    [Sentinel( SentinelIds.Sentinel_0221 )]
    public Data_0221 Data_0221;

    [Sentinel( SentinelIds.Sentinel_0222 )]
    public Data_0222 Data_0222;

    #endregion

    #region Serialization

    public static Data_021F Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_021F();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_0220:
            data.Data_0220 = Data_0220.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0221:
            data.Data_0221 = Data_0221.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0222:
            data.Data_0222 = Data_0222.Deserialize( reader, context );
            break;

          case SentinelIds.Delimiter:
            return data;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return data;
    }

    #endregion

  }

}
