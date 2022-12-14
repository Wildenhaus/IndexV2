using DirectXTexNet;
using Index.Textures;

namespace Index.Domain.Assets.Textures.Dxgi
{

  public interface IDxgiTextureAsset : ITextureAsset, IExportableAsset
  {

    #region Properties

    ScratchImage DxgiImage { get; }

    int Depth { get; }
    int MipMapCount { get; }
    int FaceCount { get; }
    DxgiTextureFormat Format { get; }

    #endregion

  }

}
