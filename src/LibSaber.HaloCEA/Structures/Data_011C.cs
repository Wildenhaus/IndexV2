using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_011C )]
  public struct Data_011C
  {

    #region Data Members

    public float Unk_00;
    public float Unk_01;

    #endregion

    #region Serialization

    public static Data_011C Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_011C
      {
        Unk_00 = reader.ReadFloat32(),
        Unk_01 = reader.ReadFloat32(),
      };
    }

    #endregion

  }

}
