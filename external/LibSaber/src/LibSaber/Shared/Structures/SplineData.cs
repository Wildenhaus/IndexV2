using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.Shared.Structures
{

  [SaberInternalName( "m3d::SPL" )]
  public struct SplineData<TOffset>
    where TOffset : unmanaged, IConvertible
  {

    #region Data Members

    /// <summary>
    ///   This is an unknown 32-bit integer.
    /// </summary>
    public int Data_00;

    /// <summary>
    ///   The type of the spline.
    /// </summary>
    [Sentinel( 0x00F0 )]
    public SplineType Type;

    /// <summary>
    ///   The size of each element in the spline data.
    /// </summary>
    /// <remarks>
    ///   If this value is 0, the data elements are uncompressed float values.
    ///   If this value is 2, the data elements are probably SNorm16 compressed.
    /// </remarks>
    [Sentinel( 0x00F1 )]
    public byte CompressedDataSize;

    [Sentinel( 0x00F2 )]
    public byte Sentinel_00F2; // Dimension X?

    [Sentinel( 0x00F3 )]
    public byte Sentinel_00F3; // Dimension Y?

    [Sentinel( 0x00F4 )]
    public int Sentinel_00F4; // Count?

    /// <summary>
    ///   The size of the data array in bytes.
    /// </summary>
    [Sentinel( 0x00F5 )]
    public int DataSizeInBytes;

    /// <summary>
    ///   The raw spline data.
    /// </summary>
    [Sentinel( 0x00F6 )]
    public byte[] Data;

    #endregion

    #region Serialization

    public static SplineData<TOffset> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var splineData = new SplineData<TOffset>();

      splineData.Data_00 = reader.ReadInt32(); // Unknown

      var sentinelReader = new SentinelReader<TOffset>( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case 0x00F0:
            splineData.Type = ( SplineType ) reader.ReadByte();
            break;
          case 0x00F1:
            splineData.CompressedDataSize = reader.ReadByte();
            break;
          case 0x00F2:
            splineData.Sentinel_00F2 = reader.ReadByte();
            break;
          case 0x00F3:
            splineData.Sentinel_00F3 = reader.ReadByte();
            break;
          case 0x00F4:
            splineData.Sentinel_00F4 = reader.ReadInt32();
            break;
          case 0x00F5:
            splineData.DataSizeInBytes = reader.ReadInt32();
            break;
          case 0x00F6:
            var dataBuffer = splineData.Data = new byte[ splineData.DataSizeInBytes ];
            reader.Read( dataBuffer );
            break;

          case 0x0001:
            return splineData;
          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return splineData;
    }

    #endregion

  }

}
