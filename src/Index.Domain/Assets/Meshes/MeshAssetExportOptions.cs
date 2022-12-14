using Index.Domain.Assets.Textures;
using Index.Domain.Assets.Textures.Dxgi;

namespace Index.Domain.Assets.Meshes
{

  public class MeshAssetExportOptionsBase<TTextureOptions> : AssetExportOptions
    where TTextureOptions : TextureExportOptions, new()
  {

    public MeshExportFormat ExportFormat { get; set; }
    public bool RemoveLODs { get; set; }
    public bool RemoveVolumes { get; set; }

    public bool ExportTextures { get; set; }
    public TTextureOptions TextureOptions { get; set; }

    public MeshAssetExportOptionsBase()
    {
      TextureOptions = new TTextureOptions();
    }

  }

  public class MeshAssetExportOptions : MeshAssetExportOptionsBase<DxgiTextureExportOptions>
  {
  }

}
