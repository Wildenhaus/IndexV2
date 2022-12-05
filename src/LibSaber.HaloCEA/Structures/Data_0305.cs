using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0305 )]
  public struct Data_0305
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_0306 )] public int Sentinel_0306;

    [Sentinel( SentinelIds.Sentinel_030C )] public int Sentinel_030C_00;
    [Sentinel( SentinelIds.Sentinel_030C )] public int Sentinel_030C_01;

    [Sentinel( SentinelIds.Sentinel_030D )] public int Sentinel_030D_00;
    [Sentinel( SentinelIds.Sentinel_030D )] public BitSet<short> Sentinel_030D_01;
    [Sentinel( SentinelIds.Sentinel_030D )] public Matrix4<float>[] Sentinel_030D_02;

    #endregion

    #region Serialization

    public static Data_0305 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0305();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_0306:
          {
            data.Sentinel_0306 = reader.ReadInt32();
            break;
          }
          case SentinelIds.Sentinel_030C:
          {
            data.Sentinel_030C_00 = reader.ReadInt32();
            data.Sentinel_030C_01 = reader.ReadInt32();
            break;
          }
          case SentinelIds.Sentinel_030D:
          {
            var count = data.Sentinel_030D_00 = reader.ReadInt32();
            var flags = data.Sentinel_030D_01 = BitSet<short>.Deserialize( reader, context );

            if ( !flags[ 0 ] )
            {
              var matrices = data.Sentinel_030D_02 = new Matrix4<float>[ count ];
              for ( var i = 0; i < count; i++ )
                matrices[ i ] = Matrix4<float>.Deserialize( reader, context );
            }

            break;
          }

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
