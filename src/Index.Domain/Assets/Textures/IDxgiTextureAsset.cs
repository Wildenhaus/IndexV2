namespace Index.Domain.Assets.Textures
{

  public interface IDxgiTextureAsset : ITextureAsset
  {

    #region Properties

    int MipMapCount { get; }
    int FaceCount { get; }
    DxgiTextureFormat Format { get; }

    #endregion

  }

}
