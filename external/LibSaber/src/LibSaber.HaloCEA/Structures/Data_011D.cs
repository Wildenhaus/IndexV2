using System.Numerics;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.ObjectBoundingBox )]
  public struct Data_011D
  {

    #region Data Members

    public int Unk_00;
    public Vector3 Unk_01;
    public Vector3 Unk_02;

    #endregion

    #region Serialization

    public static Data_011D Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_011D();

      data.Unk_00 = reader.ReadInt32();
      data.Unk_01 = reader.ReadVector3();
      data.Unk_02 = reader.ReadVector3();

      return data;
    }

    #endregion

  }

}
