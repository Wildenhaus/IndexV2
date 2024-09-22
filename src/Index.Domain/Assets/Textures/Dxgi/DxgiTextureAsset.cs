using DirectXTexNet;
using Index.Textures;

namespace Index.Domain.Assets.Textures.Dxgi
{

  [AssetExportOptionsType(typeof(DxgiTextureExportOptions))]
  public class DxgiTextureAsset : TextureAssetBase, IDxgiTextureAsset
  {

    #region Properties

    public override Type ExportOptionsType => typeof( DxgiTextureExportOptions );
    public override Type ExportJobType => typeof( DxgiTextureAssetExportJob );

    public ScratchImage DxgiImage { get; }

    public override int Width { get; }
    public override int Height { get; }
    public int Depth { get; }
    public int MipMapCount { get; }
    public int FaceCount { get; }
    public DxgiTextureFormat Format { get; }

    #endregion

    #region Constructor

    public DxgiTextureAsset(
      IAssetReference assetReference,
      TextureType textureType,
      IReadOnlyList<ITextureAssetImage> images,
      ScratchImage dxgiImage )
      : base( assetReference, textureType, images )
    {
      DxgiImage = dxgiImage;

      var metadata = dxgiImage.GetMetadata();
      Width = metadata.Width;
      Height = metadata.Height;
      Depth = metadata.Depth;
      MipMapCount = metadata.MipLevels;
      FaceCount = metadata.IsCubemap() ? metadata.ArraySize / 6 : 1;
      Format = ( DxgiTextureFormat ) metadata.Format;
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      base.OnDisposing();
      DxgiImage?.Dispose();
    }

    protected override IEnumerable<(string, string)> GetTextureInformation()
    {
      yield return ("Type", TextureType.ToString());
      yield return ("Format", Format.ToString().Replace( "DXGI_FORMAT_", "" ));
      yield return ("Dimensions", $"{Width}x{Height}x{Depth}");
      yield return ("MipMaps", MipMapCount.ToString());
      yield return ("Faces", FaceCount.ToString());
    }

    #endregion

  }

}
