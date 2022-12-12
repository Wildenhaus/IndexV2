using Index.Textures;

namespace Index.Domain.Assets.Textures.Dxgi
{

  public class DxgiTextureAsset : TextureAssetBase, IDxgiTextureAsset
  {

    #region Properties

    public DxgiImageStream DxgiStream { get; }
    public DxgiTextureInfo DxgiInfo => DxgiStream.TextureInfo;

    public override int Width => DxgiInfo.Width;
    public override int Height => DxgiInfo.Height;
    public int Depth => DxgiInfo.Depth;
    public int MipMapCount => DxgiInfo.MipCount;
    public int FaceCount => DxgiInfo.FaceCount;
    public DxgiTextureFormat Format => DxgiInfo.Format;

    #endregion

    #region Constructor

    public DxgiTextureAsset(
      IAssetReference assetReference,
      TextureType textureType,
      IReadOnlyList<ITextureAssetImage> images,
      DxgiImageStream dxgiStream )
      : base( assetReference, textureType, images )
    {
      DxgiStream = dxgiStream;
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      base.OnDisposing();
      DxgiStream?.Dispose();
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
