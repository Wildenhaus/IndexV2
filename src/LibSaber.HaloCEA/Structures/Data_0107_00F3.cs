using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( 0x00F3 )]
  public struct Data_0107_00F3
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_0103 )]
    public Data_0103 Sentinel_0103;

    [Sentinel( SentinelIds.Sentinel_0126 )]
    public bool Sentinel_0126; // Empty? just setting a bool for presence.

    #endregion

    #region Serialization

    public static Data_0107_00F3 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0107_00F3();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_0103:
            data.Sentinel_0103 = Data_0103.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0126:
            data.Sentinel_0126 = true; // Empty? just setting a bool for presence.
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
