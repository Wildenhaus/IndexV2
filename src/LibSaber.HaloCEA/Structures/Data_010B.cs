using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_010B )]
  public struct Data_010B
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_010A )]
    public Data_010A? Sentinel_010A;

    [Sentinel( SentinelIds.Sentinel_010C )]
    public int Sentinel_010C;

    [Sentinel( SentinelIds.Sentinel_010E )]
    public Data_010E? Sentinel_010E;

    [Sentinel( SentinelIds.Sentinel_0114 )]
    public Data_0114? Sentinel_0114;

    [Sentinel( SentinelIds.Sentinel_011F )]
    public Data_011F? Sentinel_011F; // might be same struct as 0114?

    [Sentinel( SentinelIds.Sentinel_01BA )]
    public string Sentinel_01BA;

    #endregion

    #region Serialization

    public static Data_010B Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_010B();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_010A:
            data.Sentinel_010A = Data_010A.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_010C:
            data.Sentinel_010C = reader.ReadInt32();
            break;
          case SentinelIds.Sentinel_010E:
            data.Sentinel_010E = Data_010E.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0114:
            data.Sentinel_0114 = Data_0114.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_011F:
            data.Sentinel_011F = Data_011F.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_01BA:
            data.Sentinel_01BA = reader.ReadNullTerminatedString();
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
