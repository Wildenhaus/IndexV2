using LibSaber.HaloCEA.Enumerations;

namespace LibSaber.HaloCEA.Files
{

  public class IPakFileEntry : CEAContainerFileEntryBase<IPakFile, IPakFileEntry>
  {

    #region Properties

    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }

    public int MipCount { get; set; }
    public int FaceCount { get; set; }

    public CEATextureFormat Format { get; set; }

    #endregion

    #region Constructor

    public IPakFileEntry( IPakFile file )
      : base( file )
    {
    }

    #endregion

  }

}
