using System.Collections;

namespace LibSaber.Halo2A.Structures
{

  public class SaberScene
  {

    #region Properties

    public uint PropertyCount { get; set; }
    public BitArray PropertyFlags { get; set; }

    public List<string> TextureList { get; set; }
    public string PS { get; set; }
    public List<string> InstMaterialInfoList { get; set; }

    public GeometryGraph GeometryGraph { get; set; }

    #endregion

  }

}
