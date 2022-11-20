using Index.Domain.Assets.Textures;

namespace Index.Profiles.HaloCEA
{

  public enum CEATextureFormat
  {
    AI88 = 0x0A,
    AI88_Alt = 0x30,
    BC1 = 0x46,
    BC2 = 0x49,
    BC3 = 0x4C,
    BC4 = 0x4F,
    BC5 = 0x52,
    ARGB8888 = 0x5A
  }

  public static class CEATextureFormatExtensions
  {

    public static DxgiTextureFormat ToDxgiFormat( this CEATextureFormat format )
    {
      switch ( format )
      {
        case CEATextureFormat.AI88:
          return DxgiTextureFormat.DXGI_FORMAT_R16G16B16A16_FLOAT;
        case CEATextureFormat.AI88_Alt:
          return DxgiTextureFormat.DXGI_FORMAT_R8G8_TYPELESS;
        case CEATextureFormat.BC1:
          return DxgiTextureFormat.DXGI_FORMAT_BC1_TYPELESS;
        case CEATextureFormat.BC2:
          return DxgiTextureFormat.DXGI_FORMAT_BC2_TYPELESS;
        case CEATextureFormat.BC3:
          return DxgiTextureFormat.DXGI_FORMAT_BC3_TYPELESS;
        case CEATextureFormat.BC4:
          return DxgiTextureFormat.DXGI_FORMAT_BC4_TYPELESS;
        case CEATextureFormat.BC5:
          return DxgiTextureFormat.DXGI_FORMAT_BC5_TYPELESS;
        case CEATextureFormat.ARGB8888:
          return DxgiTextureFormat.DXGI_FORMAT_B8G8R8A8_TYPELESS;
        default:
          throw new NotSupportedException( "Invalid CEA Texture Type." );
      }
    }

  }

}
