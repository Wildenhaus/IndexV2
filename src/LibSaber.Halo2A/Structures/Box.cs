using System.Numerics;

namespace LibSaber.Halo2A.Structures
{

  public readonly struct Box
  {

    #region Data Members

    public readonly Vector3 Min;
    public readonly Vector3 Max;

    #endregion

    #region Constructor

    public Box( Vector3 min, Vector3 max )
    {
      Min = min;
      Max = max;
    }

    #endregion

  }

}
