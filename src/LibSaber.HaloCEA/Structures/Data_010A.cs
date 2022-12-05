using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_010A )]
  public struct Data_010A
  {

    #region Data Members

    public int Unk_00;
    public int Unk_01;
    public int Unk_02;
    public byte Unk_03; // or boolean?

    #endregion

    #region Serialization

    public static Data_010A Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_010A
      {
        Unk_00 = reader.ReadInt32(),
        Unk_01 = reader.ReadInt32(),
        Unk_02 = reader.ReadInt32(),
        Unk_03 = reader.ReadByte(),
      };
    }

    #endregion

  }

}
