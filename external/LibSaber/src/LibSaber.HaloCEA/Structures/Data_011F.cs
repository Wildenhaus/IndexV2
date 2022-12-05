using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_011F )]
  public struct Data_011F
  {

    #region Data Members

    public byte Unk_00; // or boolean?
    public byte Unk_01; // or boolean?

    #endregion

    #region Serialization

    public static Data_011F Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_011F
      {
        Unk_00 = reader.ReadByte(),
        Unk_01 = reader.ReadByte(),
      };
    }

    #endregion

  }

}
