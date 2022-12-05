using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Shared.Structures
{

  public readonly struct Vector3<T>
    where T : unmanaged
  {

    #region Data Members

    public readonly T X;
    public readonly T Y;
    public readonly T Z;

    #endregion

    #region Constructor

    public Vector3( T x, T y, T z )
    {
      X = x;
      Y = y;
      Z = z;
    }

    #endregion

    #region Serialization

    public static Vector3<T> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var x = reader.ReadUnmanaged<T>();
      var y = reader.ReadUnmanaged<T>();
      var z = reader.ReadUnmanaged<T>();

      return new Vector3<T>( x, y, z );
    }

    #endregion

  }

}
