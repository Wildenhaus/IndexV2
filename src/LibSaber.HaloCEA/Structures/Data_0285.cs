using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0285 )]
  public class Data_0285
  {

    #region Data Members

    public float Unk_00;
    public float Unk_01;

    #endregion

    #region Serialization

    public static Data_0285 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0285();

      data.Unk_00 = reader.ReadFloat32();
      data.Unk_01 = reader.ReadFloat32();

      return data;
    }

    #endregion

  }

}
