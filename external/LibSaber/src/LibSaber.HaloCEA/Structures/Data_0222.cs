using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0222 )]
  public class Data_0222
  {

    #region Data Members

    public int Unk_00;
    public int Unk_01;
    public int[] Unk_02;
    public short[] Unk_03;
    public int[] Unk_04;

    #endregion

    #region Serialization

    public static Data_0222 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0222();

      data.Unk_00 = reader.ReadInt32();
      data.Unk_01 = reader.ReadInt32();

      var data_0221 = context.GetMostRecentObject<Data_0221>();

      var unk_02_count = data_0221.Unk_01 + 1;
      data.Unk_02 = new int[ unk_02_count ];
      for ( var i = 0; i < data.Unk_02.Length; i++ )
        data.Unk_02[ i ] = reader.ReadInt32();

      var unk_03_count = data.Unk_01;
      data.Unk_03 = new short[ unk_03_count ];
      for ( var i = 0; i < data.Unk_03.Length; i++ )
        data.Unk_03[ i ] = reader.ReadInt16();

      var unk_04_count = data_0221.Unk_00 + 1;
      data.Unk_04 = new int[ unk_04_count ];
      for ( var i = 0; i < data.Unk_04.Length; i++ )
        data.Unk_04[ i ] = reader.ReadInt32();

      return data;
    }

    #endregion

  }

}
