using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class BoxSerializer : H2ASerializerBase<Box>
  {

    #region Overrides

    public override Box Deserialize( NativeReader reader, ISerializationContext context )
    {
      var min = reader.ReadVector3();
      var max = reader.ReadVector3();

      return new Box( min, max );
    }

    #endregion

  }

}
