using System.Numerics;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_028C )]
  public class Data_028C
  {

    #region Data Members

    public Vector3 Unk_00;
    public Vector3 Unk_01;
    public Vector3 Unk_02;

    #endregion

    #region Serialization

    public static Data_028C Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_028C();

      data.Unk_00 = reader.ReadVector3();
      data.Unk_01 = reader.ReadVector3();
      data.Unk_02 = reader.ReadVector3();

      return data;
    }

    #endregion

  }

}
