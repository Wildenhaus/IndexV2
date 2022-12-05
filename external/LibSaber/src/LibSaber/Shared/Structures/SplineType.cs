namespace LibSaber.Shared.Structures
{

  /// <summary>
  ///   An enumeration for spline types.
  /// </summary>
  public enum SplineType : byte
  {
    Linear1D = 0,
    Linear2D = 1,
    Linear3D = 2,
    Hermit = 3,
    Bezier2D = 4,
    Bezier3D = 5,
    Lagrange = 6,
    Quat = 7,
    Color = 8
  }

}
