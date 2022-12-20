using LibSaber.Halo2A.Structures;
using LibSaber.Halo2A.Structures.Geometry;
using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Geometry
{

  public sealed class FaceSerializer : GeometryElementSerializer<Face>
  {

    #region Constructor

    public FaceSerializer( GeometryBuffer buffer )
      : base( buffer )
    {
    }

    #endregion

    #region Overrides

    public override Face Deserialize( NativeReader reader )
    {
      return new Face
      {
        A = reader.ReadUInt16(),
        B = reader.ReadUInt16(),
        C = reader.ReadUInt16(),
      };
    }

    public override IEnumerable<Face> DeserializeRange( NativeReader reader, int startIndex, int endIndex )
    {
      var startOffset = Buffer.StartOffset + ( startIndex * Buffer.ElementSize );
      var length = endIndex - startIndex;

      reader.Position = startOffset;

      for ( var i = 0; i < length; i++ )
        yield return Deserialize( reader );
    }

    #endregion

  }

}
