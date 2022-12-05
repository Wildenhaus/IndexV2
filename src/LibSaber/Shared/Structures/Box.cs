using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.Shared.Structures
{

  [SaberInternalName( "m3d::BOX" )]
  public readonly struct Box
  {

    #region Data Members

    public readonly Vector3<float> Min;
    public readonly Vector3<float> Max;

    #endregion

    #region Constructor

    public Box( Vector3<float> min, Vector3<float> max )
    {
      Min = min;
      Max = max;
    }

    #endregion

    #region Serialization

    public static Box Deserialize( NativeReader reader, ISerializationContext context )
    {
      var min = Vector3<float>.Deserialize( reader, context );
      var max = Vector3<float>.Deserialize( reader, context );

      return new Box( min, max );
    }

    #endregion

  }

}
