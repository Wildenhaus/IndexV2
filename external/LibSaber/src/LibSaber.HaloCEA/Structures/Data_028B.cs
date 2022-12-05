using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_028B )]
  public class Data_028B
  {

    #region Data Members

    public int LightType;
    public BitSet<short> Unk_01;

    #endregion

    #region Serialization

    public static Data_028B Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_028B();

      data.LightType = reader.ReadInt32();
      data.Unk_01 = BitSet<short>.Deserialize( reader, context );

      return data;
    }

    #endregion

  }

}
