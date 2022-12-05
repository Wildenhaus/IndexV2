using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_010E )]
  public struct Data_010E
  {

    #region Data Members

    public int UnkMaterialIndex; // material id?
    public int Unk_01;
    public byte Unk_02;
    public byte Unk_03;

    #endregion

    #region Serialization

    public static Data_010E Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_010E
      {
        UnkMaterialIndex = reader.ReadInt32(),
        Unk_01 = reader.ReadInt32(),
        Unk_02 = reader.ReadByte(),
        Unk_03 = reader.ReadByte(),
      };
    }

    #endregion

  }

}
