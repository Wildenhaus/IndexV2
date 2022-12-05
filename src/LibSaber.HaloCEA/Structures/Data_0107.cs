using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.ObjectSubmeshData )]
  public class Data_0107
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_0104 )]
    [Sentinel( SentinelIds.Sentinel_0137 )]
    public SentinelId SubmeshListSentinel;
    public List<Data_0104_0137> UnkSubmeshList;

    [Sentinel( SentinelIds.Sentinel_0109 )]
    public List<Data_0108> Sentinel_0109;

    [Sentinel( 0x00F3 )]
    public List<Data_0107_00F3> Sentinel_00F3;

    #endregion

    #region Serialization

    public static Data_0107 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0107();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_0104:
          case SentinelIds.Sentinel_0137:
#if DEBUG
            ASSERT( data.UnkSubmeshList is null, "Duplicate submesh data." );
#endif
            data.SubmeshListSentinel = sentinelReader.SentinelId;
            data.UnkSubmeshList = DataList<Data_0104_0137>.Deserialize( reader, context, Data_0104_0137.Deserialize );
            break;
          case SentinelIds.Sentinel_0109:
            data.Sentinel_0109 = DataList<Data_0108>.Deserialize( reader, context, Data_0108.Deserialize );
            break;

          case 0x00F3:
            data.Sentinel_00F3 = DataList<Data_0107_00F3>.Deserialize( reader, context, Data_0107_00F3.Deserialize );
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
