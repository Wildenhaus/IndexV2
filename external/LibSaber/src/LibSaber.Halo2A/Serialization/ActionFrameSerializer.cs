using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class ActionFrameSerializer : H2ASerializerBase<ActionFrame>
  {

    #region Overrides

    public override ActionFrame Deserialize( NativeReader reader, ISerializationContext context )
    {
      // TODO: This is never used?
      throw new NotImplementedException();
    }

    #endregion

  }

}
