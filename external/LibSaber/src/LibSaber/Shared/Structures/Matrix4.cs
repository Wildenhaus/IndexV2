using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Shared.Structures
{

  public readonly struct Matrix4<T>
  where T : unmanaged
  {

    #region Data Members

    public readonly T M11;
    public readonly T M12;
    public readonly T M13;
    public readonly T M14;
    public readonly T M21;
    public readonly T M22;
    public readonly T M23;
    public readonly T M24;
    public readonly T M31;
    public readonly T M32;
    public readonly T M33;
    public readonly T M34;
    public readonly T M41;
    public readonly T M42;
    public readonly T M43;
    public readonly T M44;

    #endregion

    #region Constructor

    public Matrix4( Vector4<T> m1, Vector4<T> m2, Vector4<T> m3, Vector4<T> m4 )
    {
      M11 = m1.X; M12 = m1.Y; M13 = m1.Z; M14 = m1.W;
      M21 = m2.X; M22 = m2.Y; M23 = m2.Z; M24 = m2.W;
      M31 = m3.X; M32 = m3.Y; M33 = m3.Z; M34 = m3.W;
      M41 = m4.X; M42 = m4.Y; M43 = m4.Z; M44 = m4.W;
    }

    public Matrix4(
      T m11, T m12, T m13, T m14,
      T m21, T m22, T m23, T m24,
      T m31, T m32, T m33, T m34,
      T m41, T m42, T m43, T m44 )
    {
      M11 = m11; M12 = m12; M13 = m13; M14 = m14;
      M21 = m21; M22 = m22; M23 = m23; M24 = m24;
      M31 = m31; M32 = m32; M33 = m33; M34 = m34;
      M41 = m41; M42 = m42; M43 = m43; M44 = m44;
    }

    #endregion

    #region Serialization

    public static Matrix4<T> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var m1 = Vector4<T>.Deserialize( reader, context );
      var m2 = Vector4<T>.Deserialize( reader, context );
      var m3 = Vector4<T>.Deserialize( reader, context );
      var m4 = Vector4<T>.Deserialize( reader, context );

      return new Matrix4<T>( m1, m2, m3, m4 );
    }

    #endregion

  }

}
