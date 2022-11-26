namespace Index.Domain.Assets.Textures
{

  public abstract class TextureAssetImageBase : ITextureAssetImage
  {

    #region Properties

    public int Index { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public Stream PreviewStream { get; set; }
    public IReadOnlyDictionary<TextureExportFormat, CreateTextureExportStreamDelegate> ExportDelegates { get; set; }

    #endregion

  }

}
