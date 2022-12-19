using System.Numerics;

namespace LibSaber.Halo2A.Structures
{

  public class ObjectAnimation
  {

    #region Properties

    public Vector3 IniTranslation { get; set; }
    public Spline PTranslation { get; set; }
    public Vector4 IniRotation { get; set; }
    public Spline PRotation { get; set; }
    public Vector3 IniScale { get; set; }
    public Spline PScale { get; set; }
    public float IniVisibility { get; set; }
    public Spline PVisibility { get; set; }

    #endregion

  }

}
