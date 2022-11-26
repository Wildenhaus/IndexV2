namespace Index.Domain.Assets.Textures
{

  public interface ITextureAssetImage
  {

    #region Properties

    public int Index { get; }
    int Width { get; }
    int Height { get; }

    Stream PreviewStream { get; }
    IReadOnlyDictionary<TextureExportFormat, CreateTextureExportStreamDelegate> ExportDelegates { get; }

    #endregion

  }

}
