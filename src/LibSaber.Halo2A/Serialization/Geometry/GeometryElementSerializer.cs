using LibSaber.Halo2A.Enumerations;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Geometry
{

  public abstract class GeometryElementSerializer<T>
  {
    #region Properties

    protected GeometryBuffer Buffer { get; }
    protected GeometryBufferFlags Flags => Buffer.Flags;

    #endregion

    #region Constructor

    protected GeometryElementSerializer( GeometryBuffer buffer )
    {
      Buffer = buffer;
    }

    #endregion

    #region Private Methods

    public abstract T Deserialize( NativeReader reader );

    public abstract IEnumerable<T> DeserializeRange( NativeReader reader, int startIndex, int endIndex );

    #endregion

  }

}
