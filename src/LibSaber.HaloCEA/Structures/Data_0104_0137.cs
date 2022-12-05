using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0104 )]
  [Sentinel( SentinelIds.Sentinel_0137 )]
  public struct Data_0104_0137
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_0105 )] public Data_0105? UnkFaceData;
    [Sentinel( SentinelIds.Sentinel_010B )] public List<Data_010B> UnkMaterialInfo;
    [Sentinel( SentinelIds.Sentinel_010D )] public Data_010D? UnkVertexData;
    [Sentinel( SentinelIds.Sentinel_011C )] public Data_011C? Sentinel_011C;
    [Sentinel( SentinelIds.Sentinel_0120 )] public Data_0120? Sentinel_0120;
    [Sentinel( SentinelIds.Sentinel_0128 )] public Data_0128? Sentinel_0128;
    [Sentinel( SentinelIds.Sentinel_0132 )] public Data_0132? DependencyInfo;
    [Sentinel( SentinelIds.Sentinel_0134 )] public short? SkinCompoundId;
    [Sentinel( SentinelIds.Sentinel_0138 )] public Data_0138? Sentinel_0138;


    #endregion

    #region Serialization

    public static Data_0104_0137 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0104_0137();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_0105:
            data.UnkFaceData = Data_0105.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_010B:
            data.UnkMaterialInfo = DataList<Data_010B>.Deserialize( reader, context, Data_010B.Deserialize );
            break;
          case SentinelIds.Sentinel_010D:
            data.UnkVertexData = Data_010D.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_011C:
            data.Sentinel_011C = Data_011C.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0120:
            data.Sentinel_0120 = Data_0120.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0128:
            data.Sentinel_0128 = Data_0128.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0132:
            data.DependencyInfo = Data_0132.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0134:
            data.SkinCompoundId = reader.ReadInt16();
            break;
          case SentinelIds.Sentinel_0138:
            data.Sentinel_0138 = Data_0138.Deserialize( reader, context );
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
