using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_028C )]
  public class Data_028C
  {

    #region Data Members

    public Vector3<float> Unk_00;
    public Vector3<float> Unk_01;
    public Vector3<float> Unk_02;

    #endregion

    #region Serialization

    public static Data_028C Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_028C();

      data.Unk_00 = Vector3<float>.Deserialize( reader, context );
      data.Unk_01 = Vector3<float>.Deserialize( reader, context );
      data.Unk_02 = Vector3<float>.Deserialize( reader, context );

      return data;
    }

    #endregion

  }

}
