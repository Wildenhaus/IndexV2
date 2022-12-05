using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_030F )]
  public struct Data_030F
  {

    #region Data Members

    public int Unk_00;
    public int Unk_01;
    public float Unk_02;
    public float Unk_03;
    public float Unk_04;
    public short Unk_05;

    public float[] Unk_06;

    public float Unk_07;
    public byte[] Unk_08;

    #endregion

    #region Serialization

    public static Data_030F Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_030F();

      data.Unk_00 = reader.ReadInt32();
      data.Unk_01 = reader.ReadInt32();
      data.Unk_02 = reader.ReadFloat32();
      data.Unk_03 = reader.ReadFloat32();
      data.Unk_04 = reader.ReadFloat32();
      data.Unk_05 = reader.ReadInt16();

      data.Unk_06 = new float[ 4 ];
      for ( var i = 0; i < 4; i++ )
        data.Unk_06[ i ] = reader.ReadFloat32();

      var count = data.Unk_07 = reader.ReadInt32();
      var buffer = data.Unk_08 = new byte[ ( int ) count ];
      reader.Read( buffer );

      return data;
    }

    #endregion

  }

}
