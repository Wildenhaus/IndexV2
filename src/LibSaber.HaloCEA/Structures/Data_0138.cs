using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0138 )]
  public struct Data_0138
  {

    #region Data Members

    public byte Unk_01;
    public byte Unk_02;
    public byte Unk_03;
    public byte Unk_04;
    public byte Unk_05;
    public byte Unk_06;
    public byte Unk_07;
    public byte Unk_08;
    public short Unk_09;
    public short Unk_0A;
    public int Unk_0B;

    public short Unk_0C;
    public short Unk_0D;

    public int Unk_0E;
    public int Unk_0F;
    public byte Unk_10;
    public byte Unk_11;
    public byte Unk_12;
    public byte Unk_13;

    #endregion

    #region Serialization

    public static Data_0138 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0138();

      data.Unk_01 = reader.ReadByte();
      data.Unk_02 = reader.ReadByte();
      data.Unk_03 = reader.ReadByte();
      data.Unk_04 = reader.ReadByte();
      data.Unk_05 = reader.ReadByte();
      data.Unk_06 = reader.ReadByte();
      data.Unk_07 = reader.ReadByte();
      data.Unk_08 = reader.ReadByte();
      data.Unk_09 = reader.ReadInt16();
      data.Unk_0A = reader.ReadInt16();
      data.Unk_0B = reader.ReadInt32();

      data.Unk_0C = reader.ReadInt16();
      data.Unk_0D = reader.ReadInt16();

      data.Unk_0E = reader.ReadInt32();
      data.Unk_0F = reader.ReadInt32();
      data.Unk_10 = reader.ReadByte();
      data.Unk_11 = reader.ReadByte();
      data.Unk_12 = reader.ReadByte();
      data.Unk_13 = reader.ReadByte();

      return data;
    }

    #endregion

  }

}
