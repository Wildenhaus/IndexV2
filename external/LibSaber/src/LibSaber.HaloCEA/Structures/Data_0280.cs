using System.Numerics;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.SceneLights )]
  public class SaberLightsList : List<Data_0280_Entry>
  {

    #region Constructor

    public SaberLightsList()
    {
    }

    public SaberLightsList( int capacity )
      : base( capacity )
    {
    }

    #endregion

    #region Serialization

    public static SaberLightsList Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadInt32();

      var data = new SaberLightsList( count );

      for ( var i = 0; i < count; i++ )
        data.Add( Data_0280_Entry.Deserialize( reader, context ) );

      return data;
    }

    #endregion

  }

  public class Data_0280_Entry
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_028B )]
    public Data_028B LightInfo;

    [Sentinel( SentinelIds.Sentinel_0282 )]
    public string Name;

    [Sentinel( SentinelIds.Sentinel_0283 )]
    public Matrix4x4 Matrix;

    [Sentinel( SentinelIds.Sentinel_0284 )]
    public Vector3<float> Color;

    [Sentinel( SentinelIds.Sentinel_0285 )]
    public Data_0285 Data_0285;

    [Sentinel( SentinelIds.Sentinel_0286 )]
    public Data_0286 Data_0286;

    [Sentinel( SentinelIds.Sentinel_0289 )]
    public string CubemapInfo;

    [Sentinel( SentinelIds.Sentinel_028A )]
    public string UnkMeasurementUnits;

    [Sentinel( SentinelIds.Sentinel_028C )]
    public Data_028C Data_028C;

    [Sentinel( SentinelIds.Sentinel_0348 )]
    public Data_0348 Data_0348;

    [Sentinel( SentinelIds.Sentinel_0349 )]
    public int Data_0349;

    #endregion

    #region Serialization

    public static Data_0280_Entry Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0280_Entry();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_028B:
            data.LightInfo = Data_028B.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0282:
            data.Name = reader.ReadNullTerminatedString();
            break;
          case SentinelIds.Sentinel_0283:
            data.Matrix = reader.ReadMatrix4x4();
            break;
          case SentinelIds.Sentinel_0284:
            data.Color = Vector3<float>.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0285:
            data.Data_0285 = Data_0285.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0286:
            data.Data_0286 = Data_0286.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0289:
            data.CubemapInfo = reader.ReadNullTerminatedString();
            break;
          case SentinelIds.Sentinel_028A:
            data.UnkMeasurementUnits = reader.ReadNullTerminatedString();
            break;
          case SentinelIds.Sentinel_028C:
            data.Data_028C = Data_028C.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0348:
            data.Data_0348 = Data_0348.Deserialize( reader, context );
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
