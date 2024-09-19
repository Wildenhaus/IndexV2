namespace Index.Textures
{

  public struct DxgiTextureInfo
  {

    #region Data Members

    public int Width;
    public int Height;
    public int Depth;

    public int FaceCount;
    public int MipCount;
    public bool IsNotCubeMapOverride;

    public DxgiTextureFormat Format;

    #endregion

    #region Properties

    public bool IsCubeMap => FaceCount >= 6;

    #endregion

  }

}
