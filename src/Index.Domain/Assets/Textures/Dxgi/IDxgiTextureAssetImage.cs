using Index.Textures;

namespace Index.Domain.Assets.Textures.Dxgi
{

  public interface IDxgiTextureAssetImage : ITextureAssetImage
  {

    #region Properties

    public int Depth { get; }
    public int MipMapCount { get; }
    public int FaceCount { get; }
    public DxgiTextureFormat Format { get; }

    #endregion

  }

}
