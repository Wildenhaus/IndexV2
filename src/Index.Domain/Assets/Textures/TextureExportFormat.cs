namespace Index.Domain.Assets.Textures
{

  public enum TextureExportFormat
  {

    DDS,
    TGA,
    PNG,
    HDR,
    BMP,
    TIFF,
    JPEG,

  }

  public static class TextureExportFormatExtensions
  {

    public static string GetFileExtension( this TextureExportFormat format )
    {
      switch ( format )
      {
        default:
          return $".{format.ToString().ToLower()}";
      }
    }

  }

}
