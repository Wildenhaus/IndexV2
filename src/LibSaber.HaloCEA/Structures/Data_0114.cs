using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0114 )]
  public struct Data_0114
  {

    #region Data Members

    public byte Unk_00; // or boolean?
    public byte Unk_01; // or boolean?

    #endregion

    #region Serialization

    public static Data_0114 Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_0114
      {
        Unk_00 = reader.ReadByte(),
        Unk_01 = reader.ReadByte(),
      };
    }

    #endregion

  }

}
