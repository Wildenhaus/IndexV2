namespace Index.Domain.Assets.Textures
{

  public abstract class TextureAssetImageBase : DisposableObject, ITextureAssetImage
  {

    #region Properties

    public int Index { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }

    public Stream PreviewStream { get; set; }
    public IReadOnlyDictionary<TextureExportFormat, CreateTextureExportStreamDelegate> ExportDelegates { get; set; }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      PreviewStream?.Dispose();
    }

    #endregion

  }

}
