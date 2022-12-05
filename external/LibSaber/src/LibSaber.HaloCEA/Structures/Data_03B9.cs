using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.ObjectInfo )]
  public class Data_03B9
  {

    #region Data Members

    public string Name;
    public short Id;
    public BitSet<short> Flags;
    public int VertexCount;
    public int FaceCount;

    #endregion

    #region Serialization

    public static Data_03B9 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_03B9();

      data.Name = reader.ReadNullTerminatedString();
      data.Id = reader.ReadInt16();
      data.Flags = BitSet<short>.Deserialize( reader, context );
      data.VertexCount = reader.ReadInt32();
      data.FaceCount = reader.ReadInt32();

      return data;
    }

    #endregion

  }

}
