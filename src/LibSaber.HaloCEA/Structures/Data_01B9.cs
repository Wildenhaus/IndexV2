using System.Numerics;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_01B9 )]
  public class Data_01B9
  {

    #region Data Members

    public string InstanceName;
    public string TemplateName;
    public string Unk_02;
    public string Unk_03;
    public Matrix4x4 Matrix;

    [Sentinel( SentinelIds.Sentinel_01BC )]
    public int Data_01BC;

    [Sentinel( SentinelIds.Sentinel_01BD )]
    public short Data_01BD;

    #endregion

    #region Serialization

    public static Data_01B9 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_01B9();

      const int MAGIC_ANIS = 0x41494E53; // ANIS
      ASSERT( reader.ReadInt32() == MAGIC_ANIS, "Invalid ANIS magic." );

      data.InstanceName = reader.ReadNullTerminatedString();
      data.TemplateName = reader.ReadNullTerminatedString();
      data.Unk_02 = reader.ReadNullTerminatedString();
      data.Unk_03 = reader.ReadNullTerminatedString();
      data.Matrix = reader.ReadMatrix4x4();

      var sentinelReader = new SentinelReader( reader );
      sentinelReader.Next();
      ASSERT( sentinelReader.SentinelId == SentinelIds.Sentinel_01BC );
      data.Data_01BC = reader.ReadInt32();

      sentinelReader.Next();
      ASSERT( sentinelReader.SentinelId == SentinelIds.Sentinel_01BD );
      data.Data_01BD = reader.ReadInt16();

      return data;
    }

    #endregion

  }

}
