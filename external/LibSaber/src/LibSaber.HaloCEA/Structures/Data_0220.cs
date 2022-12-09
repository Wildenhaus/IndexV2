using System.Numerics;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0220 )]
  public class Data_0220
  {

    #region Data Members

    public Vector3<int> Unk_00;
    public Vector3 Unk_01;
    public Vector3 Unk_02;
    public Vector3 Unk_03;

    #endregion

    #region Serialization

    public static Data_0220 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0220();

      data.Unk_00 = Vector3<int>.Deserialize( reader, context );
      data.Unk_01 = reader.ReadVector3();
      data.Unk_02 = reader.ReadVector3();
      data.Unk_03 = reader.ReadVector3();

      context.AddObject( data );
      return data;
    }

    #endregion

  }

}
