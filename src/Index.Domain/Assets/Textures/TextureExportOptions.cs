using Index.Textures;

namespace Index.Domain.Assets.Textures
{

  public abstract class TextureExportOptions : AssetExportOptions
  {

    public ImageFormat ExportFormat { get; set; }
    public NormalMapFormat NormalMapFormat { get; set; }
    public bool RecalculateZChannel { get; set; }
    public bool ExportAdditionalData { get; set; }

  }

}
