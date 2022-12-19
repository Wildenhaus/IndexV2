using System.Numerics;

namespace LibSaber.Halo2A.Structures
{

  public class AnimationRooted
  {

    #region Properties

    public Vector3 IniTranslation { get; set; }
    public Spline PTranslation { get; set; }
    public Vector4 IniRotation { get; set; }
    public Spline PRotation { get; set; }

    #endregion

  }

}
