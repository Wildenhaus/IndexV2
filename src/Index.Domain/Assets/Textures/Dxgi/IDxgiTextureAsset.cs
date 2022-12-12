using Index.Textures;

namespace Index.Domain.Assets.Textures.Dxgi
{

  public interface IDxgiTextureAsset : ITextureAsset
  {

    #region Properties

    int Depth { get; }
    int MipMapCount { get; }
    int FaceCount { get; }
    DxgiTextureFormat Format { get; }

    #endregion

  }

}
