using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Structures
{

  public abstract class Spline
  {
    /* This is the base class for splines.
     * Every type of spline is serialized the same, but the shape
     * of the data will be different based on the actual type.
     * 
     * Derived classes can implement the functionality to deliver
     * the appropriate data based on the internal spline data.
     */

    #region Data Members

    /// <summary>
    ///   The serial spline data.
    /// </summary>
    protected readonly SplineData _data;

    #endregion

    #region Properties

    /// <summary>
    ///   The spline type.
    /// </summary>
    public abstract SplineType Type { get; }

    /// <summary>
    ///   The number of elements in the spline.
    /// </summary>
    public uint Count
    {
      [MethodImpl( MethodImplOptions.AggressiveInlining )]
      get => _data.Count;
    }

    #endregion

    #region Constructor

    /// <summary>
    ///   Constructs a new <see cref="Spline" />.
    /// </summary>
    /// <param name="data">
    ///   The spline data.
    /// </param>
    protected Spline( SplineData data )
    {
      ASSERT( data.SplineType == Type,
        "Provided SplineData ({0}) is not of type {1}.",
        data.SplineType, Type );

      _data = data;
    }

    #endregion

  }

  /// <summary>
  ///   A Linear 1D Spline structure.
  /// </summary>
  public sealed class SplineLinear1D : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Linear1D;

    #endregion

    #region Constructor

    public SplineLinear1D( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A Linear 2D Spline structure.
  /// </summary>
  public class SplineLinear2D : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Linear2D;

    #endregion

    #region Constructor

    public SplineLinear2D( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A Linear 3D Spline structure.
  /// </summary>
  public class SplineLinear3D : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Linear3D;

    #endregion

    #region Constructor

    public SplineLinear3D( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A Hermit Spline structure.
  /// </summary>
  public class SplineHermit : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Hermit;

    #endregion

    #region Constructor

    public SplineHermit( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A 2D Bezier Spline structure.
  /// </summary>
  public class SplineBezier2D : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Bezier2D;

    #endregion

    #region Constructor

    public SplineBezier2D( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A 3D Bezier Spline structure.
  /// </summary>
  public class SplineBezier3D : Spline
  {

    #region Properties

    // TODO: Implement data accessors.

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Bezier3D;

    #endregion

    #region Constructor

    public SplineBezier3D( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A Lagrange Spline structure.
  /// </summary>
  public class SplineLagrange : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Lagrange;

    #endregion

    #region Constructor

    public SplineLagrange( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A Quaternarion Spline structure.
  /// </summary>
  public class SplineQuat : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Quat;

    #endregion

    #region Constructor

    public SplineQuat( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

  /// <summary>
  ///   A Color Spline structure.
  /// </summary>
  public class SplineColor : Spline
  {

    // TODO: Implement data accessors.

    #region Properties

    /// <inheritdoc cref="Spline.Type" />
    public override SplineType Type => SplineType.Color;

    #endregion

    #region Constructor

    public SplineColor( SplineData splineData )
      : base( splineData )
    {
    }

    #endregion

  }

}
