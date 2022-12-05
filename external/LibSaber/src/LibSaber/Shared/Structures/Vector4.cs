using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Shared.Structures
{

  public readonly struct Vector4<T>
  where T : unmanaged
  {

    #region Data Members

    public readonly T X;
    public readonly T Y;
    public readonly T Z;
    public readonly T W;

    #endregion

    #region Constructor

    public Vector4( T x, T y, T z, T w )
    {
      X = x;
      Y = y;
      Z = z;
      W = w;
    }

    #endregion

    #region Serialization

    public static Vector4<T> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var x = reader.ReadUnmanaged<T>();
      var y = reader.ReadUnmanaged<T>();
      var z = reader.ReadUnmanaged<T>();
      var w = reader.ReadUnmanaged<T>();

      return new Vector4<T>( x, y, z, w );
    }

    #endregion

  }

}
