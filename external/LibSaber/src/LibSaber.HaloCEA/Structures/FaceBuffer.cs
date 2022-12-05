using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.HaloCEA.Structures
{

  public class FaceBuffer : List<Face>
  {

    #region Constructor

    public FaceBuffer()
    {
    }

    public FaceBuffer( int capacity )
      : base( capacity )
    {
    }

    #endregion

    #region Serialization

    public static FaceBuffer Deserialize( NativeReader reader, ISerializationContext context )
    {
      var faceCount = reader.ReadInt32();
      var buffer = new FaceBuffer( faceCount );

      for ( var i = 0; i < faceCount; i++ )
      {
        var face = new Face(
          reader.ReadInt16(),
          reader.ReadInt16(),
          reader.ReadInt16()
          );

        buffer.Add( face );
      }

      return buffer;
    }

    #endregion

  }

}
