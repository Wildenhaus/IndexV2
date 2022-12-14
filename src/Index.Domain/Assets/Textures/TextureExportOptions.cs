namespace Index.Domain.Assets.Textures
{

  public class TextureExportOptions : AssetExportOptions
  {

    public TextureExportFormat ExportFormat { get; set; }
    public NormalMapFormat NormalMapFormat { get; set; }
    public bool RecalculateZChannel { get; set; }

  }

}
