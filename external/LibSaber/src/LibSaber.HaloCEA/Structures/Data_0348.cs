using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0348 )]
  public class Data_0348
  {

    #region Data Members

    public int Unk_00;
    public string Unk_01;

    [Sentinel( SentinelIds.Sentinel_0349 )]
    public int Data_0349;

    #endregion

    #region Serialization

    public static Data_0348 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0348();

      data.Unk_00 = reader.ReadInt32();
      data.Unk_01 = reader.ReadNullTerminatedString();

      var sentinelReader = new SentinelReader( reader );

      sentinelReader.Next();
      ASSERT( sentinelReader.SentinelId == SentinelIds.Sentinel_0349 );
      data.Data_0349 = reader.ReadInt32();

      return data;
    }

    #endregion

  }

}