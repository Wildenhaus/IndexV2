using Index.Textures;

namespace Index.Domain.Assets.Textures.Dxgi
{

  public class DxgiTextureAsset : TextureAssetBase, IDxgiTextureAsset
  {

    #region Properties

    public int Depth { get; set; }
    public int MipMapCount { get; set; }
    public int FaceCount { get; set; }
    public DxgiTextureFormat Format { get; set; }

    #endregion

    #region Constructor

    public DxgiTextureAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

    #region Overrides

    protected override IEnumerable<(string, string)> GetTextureInformation()
    {
      yield return ("Format", Format.ToString().Replace( "DXGI_FORMAT_", "" ));
      yield return ("Dimensions", $"{Width}x{Height}x{Depth}");
      yield return ("MipMaps", MipMapCount.ToString());
      yield return ("Faces", FaceCount.ToString());
    }

    #endregion

  }

}
