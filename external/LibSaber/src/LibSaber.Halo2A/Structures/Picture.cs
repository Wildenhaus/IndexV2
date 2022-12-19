using LibSaber.Halo2A.Enumerations;

namespace LibSaber.Halo2A.Structures
{

  public class Picture
  {

    #region Properties

    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
    public int Faces { get; set; }
    public int MipMapCount { get; set; }
    public PictureFormat Format { get; set; }

    public byte[] Data { get; set; }

    #endregion

  }

}
