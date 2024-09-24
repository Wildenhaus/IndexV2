using Assimp;

namespace Index.Profiles.SpaceMarine2.Common
{

  public static class AssimpHelper
  {

    #region Constants

    private const float MetersToFeet = .0328084f;

    #endregion

    #region Public Methods

    public static Assimp.Matrix4x4 ToAssimp( this System.Numerics.Matrix4x4 m )
    {
      return new Matrix4x4(
          m.M11, m.M12, m.M13, m.M14,
          m.M21, m.M22, m.M23, m.M24,
          m.M31, m.M32, m.M33, m.M34,
          m.M41, m.M42, m.M43, m.M44
          );
    }

    public static Assimp.Vector3D ToAssimp( this System.Numerics.Vector3 v )
    {
      return new Vector3D( v.X, v.Y, v.Z );
    }

    public static Assimp.Vector3D ToAssimpVector3D( this System.Numerics.Vector4 v )
    {
      return new Vector3D( v.X, v.Y, v.Z );
    }

    public static Assimp.Color4D ToAssimpColor4D( this System.Numerics.Vector4 c )
    {
      return new Color4D(
        c.X,
        c.Y,
        c.Z,
        c.W
        );
    }

    public static System.Numerics.Matrix4x4 ToNumerics( this Assimp.Matrix4x4 m )
    {
      return new System.Numerics.Matrix4x4(
        m.A1, m.A2, m.A3, m.A4,
        m.B1, m.B2, m.B3, m.B4,
        m.C1, m.C2, m.C3, m.C4,
        m.D1, m.D2, m.D3, m.D4
        );
    }

    public static IEnumerable<Node> EnumerateChildren( this Node node )
    {
      foreach ( var child in node.Children )
        yield return child;
    }

    public static IEnumerable<Node> TraverseChildren( this Node node )
    {
      foreach ( var child in node.Children )
      {
        yield return child;
        foreach ( var childOfChild in TraverseChildren( child ) )
          yield return childOfChild;
      }

    }

    #endregion

  }

}
