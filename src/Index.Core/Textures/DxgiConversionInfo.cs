using DirectXTexNet;

namespace Index.Textures
{

  public class DxgiConversionInfo
  {

    #region Properties

    public string FormatId { get; }
    public DXGI_FORMAT DefaultFormat { get; }
    public IReadOnlySet<DXGI_FORMAT> SupportedFormats { get; }

    #endregion

    #region Constructor

    private DxgiConversionInfo( string formatId, DXGI_FORMAT defaultFormat, IEnumerable<DXGI_FORMAT> supportedFormats )
    {
      FormatId = formatId;
      DefaultFormat = defaultFormat;
      SupportedFormats = supportedFormats.ToHashSet();
    }

    public static DxgiConversionInfo Define( string formatId, DXGI_FORMAT defaultFormat, IEnumerable<DXGI_FORMAT> supportedFormats )
    {
      return new DxgiConversionInfo( formatId, defaultFormat, supportedFormats );
    }

    #endregion

    #region Public Methods

    public bool SupportsFormat( DXGI_FORMAT format )
      => SupportedFormats.Contains( format );

    #endregion

    #region Definitions

    public static readonly DxgiConversionInfo TGA = DxgiConversionInfo.Define(
      formatId: "TGA",
      defaultFormat: DXGI_FORMAT.R8G8B8A8_UNORM,
      supportedFormats: new DXGI_FORMAT[]
      {
          DXGI_FORMAT.R8G8B8A8_UNORM,
          DXGI_FORMAT.R8G8B8A8_UNORM_SRGB,
          DXGI_FORMAT.R8_UNORM,
          DXGI_FORMAT.A8_UNORM,
          DXGI_FORMAT.B5G5R5A1_UNORM,
          DXGI_FORMAT.B8G8R8A8_UNORM,
          DXGI_FORMAT.B8G8R8X8_UNORM,
          DXGI_FORMAT.B8G8R8A8_UNORM_SRGB,
          DXGI_FORMAT.B8G8R8X8_UNORM_SRGB,
      } );

    public static readonly DxgiConversionInfo PNG = DxgiConversionInfo.Define(
      formatId: "PNG",
      defaultFormat: DXGI_FORMAT.R32G32B32A32_FLOAT,
      supportedFormats: new DXGI_FORMAT[]
      {
        DXGI_FORMAT.R32G32B32A32_FLOAT,
        DXGI_FORMAT.R32G32B32_FLOAT,
        DXGI_FORMAT.R16G16B16A16_FLOAT,
        DXGI_FORMAT.R16G16B16A16_UNORM,
        DXGI_FORMAT.R10G10B10A2_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM_SRGB,
        DXGI_FORMAT.D32_FLOAT,
        DXGI_FORMAT.R32_FLOAT,
        DXGI_FORMAT.R16_FLOAT,
        DXGI_FORMAT.D16_UNORM,
        DXGI_FORMAT.R16_UNORM,
        DXGI_FORMAT.R8_UNORM,
        DXGI_FORMAT.A8_UNORM,
        DXGI_FORMAT.B5G6R5_UNORM,
        DXGI_FORMAT.B5G5R5A1_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM,
        DXGI_FORMAT.B8G8R8X8_UNORM,
        DXGI_FORMAT.R10G10B10_XR_BIAS_A2_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM_SRGB,
        DXGI_FORMAT.B8G8R8X8_UNORM_SRGB,
      } );

    public static readonly DxgiConversionInfo HDR = DxgiConversionInfo.Define(
      formatId: "HDR",
      defaultFormat: DXGI_FORMAT.R32G32B32A32_FLOAT,
      supportedFormats: new DXGI_FORMAT[]
      {
        DXGI_FORMAT.R32G32B32A32_FLOAT,
        DXGI_FORMAT.R32G32B32_FLOAT,
        DXGI_FORMAT.R16G16B16A16_FLOAT,
      } );

    public static readonly DxgiConversionInfo JPEG = DxgiConversionInfo.Define(
      formatId: "JPEG",
      defaultFormat: DXGI_FORMAT.R32G32B32A32_FLOAT,
      supportedFormats: new DXGI_FORMAT[]
      {
        DXGI_FORMAT.R32G32B32A32_FLOAT,
        DXGI_FORMAT.R32G32B32_FLOAT,
        DXGI_FORMAT.R16G16B16A16_FLOAT,
        DXGI_FORMAT.R16G16B16A16_UNORM,
        DXGI_FORMAT.R10G10B10A2_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM_SRGB,
        DXGI_FORMAT.D32_FLOAT,
        DXGI_FORMAT.R32_FLOAT,
        DXGI_FORMAT.R16_FLOAT,
        DXGI_FORMAT.D16_UNORM,
        DXGI_FORMAT.R16_UNORM,
        DXGI_FORMAT.R8_UNORM,
        DXGI_FORMAT.A8_UNORM,
        DXGI_FORMAT.B5G6R5_UNORM,
        DXGI_FORMAT.B5G5R5A1_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM,
        DXGI_FORMAT.B8G8R8X8_UNORM,
        DXGI_FORMAT.R10G10B10_XR_BIAS_A2_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM_SRGB,
        DXGI_FORMAT.B8G8R8X8_UNORM_SRGB,
      } );

    public static readonly DxgiConversionInfo BMP = DxgiConversionInfo.Define(
      formatId: "BMP",
      defaultFormat: DXGI_FORMAT.R32G32B32A32_FLOAT,
      supportedFormats: new DXGI_FORMAT[]
      {
        DXGI_FORMAT.R32G32B32A32_FLOAT,
        DXGI_FORMAT.R32G32B32_FLOAT,
        DXGI_FORMAT.R16G16B16A16_FLOAT,
        DXGI_FORMAT.R16G16B16A16_UNORM,
        DXGI_FORMAT.R10G10B10A2_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM_SRGB,
        DXGI_FORMAT.D32_FLOAT,
        DXGI_FORMAT.R32_FLOAT,
        DXGI_FORMAT.R16_FLOAT,
        DXGI_FORMAT.D16_UNORM,
        DXGI_FORMAT.R16_UNORM,
        DXGI_FORMAT.R8_UNORM,
        DXGI_FORMAT.A8_UNORM,
        DXGI_FORMAT.B5G6R5_UNORM,
        DXGI_FORMAT.B5G5R5A1_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM,
        DXGI_FORMAT.B8G8R8X8_UNORM,
        DXGI_FORMAT.R10G10B10_XR_BIAS_A2_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM_SRGB,
        DXGI_FORMAT.B8G8R8X8_UNORM_SRGB,
      } );

    public static readonly DxgiConversionInfo TIFF = DxgiConversionInfo.Define(
      formatId: "TIFF",
      defaultFormat: DXGI_FORMAT.R32G32B32A32_FLOAT,
      supportedFormats: new DXGI_FORMAT[]
      {
        DXGI_FORMAT.R32G32B32A32_FLOAT,
        DXGI_FORMAT.R32G32B32_FLOAT,
        DXGI_FORMAT.R16G16B16A16_FLOAT,
        DXGI_FORMAT.R16G16B16A16_UNORM,
        DXGI_FORMAT.R10G10B10A2_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM,
        DXGI_FORMAT.R8G8B8A8_UNORM_SRGB,
        DXGI_FORMAT.D32_FLOAT,
        DXGI_FORMAT.R32_FLOAT,
        DXGI_FORMAT.R16_FLOAT,
        DXGI_FORMAT.D16_UNORM,
        DXGI_FORMAT.R16_UNORM,
        DXGI_FORMAT.R8_UNORM,
        DXGI_FORMAT.A8_UNORM,
        DXGI_FORMAT.B5G6R5_UNORM,
        DXGI_FORMAT.B5G5R5A1_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM,
        DXGI_FORMAT.B8G8R8X8_UNORM,
        DXGI_FORMAT.R10G10B10_XR_BIAS_A2_UNORM,
        DXGI_FORMAT.B8G8R8A8_UNORM_SRGB,
        DXGI_FORMAT.B8G8R8X8_UNORM_SRGB,
      } );

    public static readonly DxgiConversionInfo POST_PROCESS = DxgiConversionInfo.Define(
      formatId: "HDR",
      defaultFormat: DXGI_FORMAT.R16G16B16A16_FLOAT,
      supportedFormats: new DXGI_FORMAT[]
      {
        DXGI_FORMAT.R16G16B16A16_FLOAT,
      } );

    #endregion

  }

}
