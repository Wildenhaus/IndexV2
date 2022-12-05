using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0284 )]
  public class Data_0284
  {

    #region Data Members

    public int Unk_00;
    public int Unk_01;
    public int Unk_02;

    #endregion

    #region Serialization

    public static Data_0284 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0284();

      data.Unk_00 = reader.ReadInt32();
      data.Unk_01 = reader.ReadInt32();
      data.Unk_02 = reader.ReadInt32();

      return data;
    }

    #endregion

  }

}
