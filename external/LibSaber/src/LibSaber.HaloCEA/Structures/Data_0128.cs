using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0128 )]
  public struct Data_0128
  {

    #region Data Members

    public byte Unk_01;
    public byte Unk_02;
    public byte Unk_03;
    public byte Unk_04;
    public byte Unk_05;
    public byte Unk_06;
    public byte UnkType;
    public byte Unk_08;
    public short UnkVertexCount;
    public short Unk_09;
    public int Unk_0B;

    public int Unk_0E;
    public int Unk_0F;
    public byte Unk_10;
    public byte NumberOfDependents;
    public byte Unk_12;
    public byte Unk_13;

    #endregion

    #region Serialization

    public static Data_0128 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0128();

      data.Unk_01 = reader.ReadByte();
      data.Unk_02 = reader.ReadByte();
      data.Unk_03 = reader.ReadByte();
      data.Unk_04 = reader.ReadByte();
      data.Unk_05 = reader.ReadByte();
      data.Unk_06 = reader.ReadByte();
      data.UnkType = reader.ReadByte(); // 0x10 = mesh? 0x12 = skin? 0x13 = skincompound
      data.Unk_08 = reader.ReadByte();
      data.UnkVertexCount = reader.ReadInt16();
      data.Unk_09 = reader.ReadInt16(); // face count * 3?
      data.Unk_0B = reader.ReadInt32();

      data.Unk_0E = reader.ReadInt32();
      data.Unk_0F = reader.ReadInt32();
      data.Unk_10 = reader.ReadByte();
      data.NumberOfDependents = reader.ReadByte();
      data.Unk_12 = reader.ReadByte();
      data.Unk_13 = reader.ReadByte();

      return data;
    }

    #endregion

  }

}
