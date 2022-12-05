using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0105 )]
  public struct Data_0105
  {

    #region Data Members

    public int FaceOffset;
    public int FaceCount;

    #endregion

    #region Serialization

    public static Data_0105 Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_0105
      {
        FaceOffset = reader.ReadInt32(),
        FaceCount = reader.ReadInt32(),
      };
    }

    #endregion

  }

}
