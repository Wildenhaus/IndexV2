using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.HaloCEA.Structures
{

  public struct LodDefinition
  {

    public short ObjectId;
    public byte Index;
    public bool IsLastLodUpToInfinity;

    public static LodDefinition Deserialize( NativeReader reader, ISerializationContext context )
    {
      return new LodDefinition
      {
        ObjectId = reader.ReadInt16(),
        Index = reader.ReadByte(),
        IsLastLodUpToInfinity = reader.ReadBoolean()
      };
    }

  }

}
