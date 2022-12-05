using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0108 )]
  public struct Data_0108
  {

    #region Data Members

    public byte Unk_00;
    public short Unk_01;
    public byte Unk_02;

    #endregion

    #region Serialization

    public static Data_0108 Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_0108
      {
        Unk_00 = reader.ReadByte(),
        Unk_01 = reader.ReadInt16(),
        Unk_02 = reader.ReadByte()
      };
    }

    #endregion

  }

}
