using LibSaber.Halo2A.Structures;
using LibSaber.Halo2A.Structures.Geometry;
using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Geometry
{

  public sealed class FaceSerializer : GeometryElementSerializer<Face>
  {

    #region Constructor

    public FaceSerializer( NativeReader reader, GeometryBuffer buffer )
      : base( reader, buffer )
    {
    }

    #endregion

    #region Overrides

    protected override Face ReadElement()
    {
      return new Face
      {
        A = Reader.ReadInt16(),
        B = Reader.ReadInt16(),
        C = Reader.ReadInt16()
      };
    }

    #endregion

  }

}
