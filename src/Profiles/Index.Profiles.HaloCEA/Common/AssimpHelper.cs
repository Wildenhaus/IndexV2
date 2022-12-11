using Assimp;

namespace Index.Profiles.HaloCEA.Common
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

    public static Assimp.Matrix4x4 GetSceneRootTransform()
    {
      var rot = System.Numerics.Matrix4x4.CreateFromAxisAngle( new System.Numerics.Vector3( 0, 1, 0 ), 0 );
      return rot.ToAssimp();
    }

    #endregion

  }

}
