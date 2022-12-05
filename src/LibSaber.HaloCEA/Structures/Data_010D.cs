using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_010D )]
  public struct Data_010D
  {

    #region Data Members

    public int VertexOffset;
    public int VertexCount;

    #endregion

    #region Serialization

    public static Data_010D Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new Data_010D
      {
        VertexOffset = reader.ReadInt32(),
        VertexCount = reader.ReadInt32(),
      };
    }

    #endregion

  }

}
