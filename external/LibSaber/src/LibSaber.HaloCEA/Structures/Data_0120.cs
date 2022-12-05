using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0120 )]
  public struct Data_0120
  {

    #region Data Members

    public float Unk_00;
    public float Unk_01;
    public float Unk_02;
    public float Unk_03;
    public float Unk_04;
    public float Unk_05;
    public float Unk_06;
    public float Unk_07;

    #endregion

    #region Serialization

    public static Data_0120 Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_0120
      {
        Unk_00 = reader.ReadFloat32(),
        Unk_01 = reader.ReadFloat32(),
        Unk_02 = reader.ReadFloat32(),
        Unk_03 = reader.ReadFloat32(),
        Unk_04 = reader.ReadFloat32(),
        Unk_05 = reader.ReadFloat32(),
        Unk_06 = reader.ReadFloat32(),
        Unk_07 = reader.ReadFloat32(),
      };
    }

    #endregion

  }

}
