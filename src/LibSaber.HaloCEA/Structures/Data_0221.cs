using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0221 )]
  public class Data_0221
  {

    #region Data Members

    public int Unk_00;
    public int Unk_01;
    public int[] Unk_02;
    public short[] Unk_03;

    #endregion

    #region Serialization

    public static Data_0221 Deserialize( NativeReader reader, ISerializationContext context )
    {
      /* Not sure what this is, but it looks like a bunch of zero data.
       * 
       * The number of bytes is calculated based on the scalar product of 
       * the first vector in the previous sentinel (0x0220):
       *    Data_0220.Unk_00.X * Data_0220.Unk_00.Y * Unk_0220.Unk_00.Z + 1;
       * 
       * This data is an array of 4-byte elements. Probably either int or float,
       * but since my sample data is all zeros, I can't tell.
       */

      var data = new Data_0221();

      data.Unk_00 = reader.ReadInt32();
      data.Unk_01 = reader.ReadInt32();

      var data_0220 = context.GetMostRecentObject<Data_0220>();
      var vectorProduct = data_0220.Unk_00.X * data_0220.Unk_00.Y * data_0220.Unk_00.Z;

      var unk_02_count = vectorProduct + 1;
      data.Unk_02 = new int[ unk_02_count ];
      for ( var i = 0; i < unk_02_count; i++ )
        data.Unk_02[ i ] = reader.ReadInt32();

      data.Unk_03 = new short[ data.Unk_01 ];
      for ( var i = 0; i < data.Unk_03.Length; i++ )
        data.Unk_03[ i ] = reader.ReadInt16();

      context.AddObject( data );
      return data;
    }

    #endregion

  }

}
