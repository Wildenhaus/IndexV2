using Assimp;
using LibSaber.Extensions;
using LibSaber.Shared.Structures;

namespace Index.Profiles.HaloCEA.Common
{

  public static class AssimpHelper
  {

    public static Matrix4x4 ComposeTransformMatrix(
      Vector3<float> translation,
      Vector3<float> scale,
      Vector4<float> rotation )
    {
      var t = translation;
      var s = scale;
      var r = rotation.QuatToRotationMatrix();

      return new Matrix4x4(
        s.X, r.A2, r.A3, t.X,
        r.B1, s.Y, r.B3, t.Y,
        r.C1, r.C2, s.Z, t.Z,
        0, 0, 0, 1
        );
    }

    public static Matrix4x4 ToMatrix4( this Matrix4<float> m )
    {
      return new Matrix4x4(
          m.M11, m.M12, m.M13, m.M14,
          m.M21, m.M22, m.M23, m.M24,
          m.M31, m.M32, m.M33, m.M34,
          m.M41, m.M42, m.M43, m.M44
          );
    }

    public static Vector3D ToVector3D( this Vector3<float> v )
    {
      return new Vector3D( v.X, v.Y, v.Z );
    }

    public static Vector3D ToVector3D( this Vector4<float> v )
    {
      return new Vector3D( v.X, v.Y, v.Z );
    }

    public static Color4D ToColor4D( this Vector4<byte> c )
    {
      return new Color4D(
        c.X.UNormToFloat(),
        c.Y.UNormToFloat(),
        c.Z.UNormToFloat(),
        c.W.UNormToFloat()
        );
    }

    public static Matrix3x3 QuatToRotationMatrix( this Vector4<float> quat )
    {
      var q0 = quat.X;
      var q1 = quat.Y;
      var q2 = quat.Z;
      var q3 = quat.W;

      var r00 = 2 * ( q0 * q0 + q1 * q1 ) - 1;
      var r01 = 2 * ( q1 * q2 - q0 * q3 );
      var r02 = 2 * ( q1 * q3 + q0 * q2 );

      var r10 = 2 * ( q1 * q2 + q0 * q3 );
      var r11 = 2 * ( q0 * q0 + q2 * q2 ) - 1;
      var r12 = 2 * ( q2 * q3 - q0 * q1 );

      var r20 = 2 * ( q1 * q3 - q0 * q2 );
      var r21 = 2 * ( q2 * q3 + q0 * q1 );
      var r22 = 2 * ( q0 * q0 + q3 * q3 ) - 1;

      return new Matrix3x3(
        r00, r01, r02,
        r10, r11, r12,
        r20, r21, r22
        );
    }

  }

}
