using ImageMagick;

namespace Index.Textures
{

  public enum ImageFormat
  {

    DDS,
    TGA,
    PNG,
    HDR,
    BMP,
    TIFF,
    JPEG,

  }

  public static class ImageFormatExtensions
  {

    public static string GetFileExtension( this ImageFormat format )
    {
      switch ( format )
      {
        default:
          return $".{format.ToString().ToLower()}";
      }
    }

    internal static MagickFormat GetMagickFormat( this ImageFormat format )
    {
      switch ( format )
      {
        case ImageFormat.DDS:
          return MagickFormat.Dds;
        case ImageFormat.TGA:
          return MagickFormat.Tga;
        case ImageFormat.PNG:
          return MagickFormat.Png;
        case ImageFormat.HDR:
          return MagickFormat.Hdr;
        case ImageFormat.BMP:
          return MagickFormat.Bmp;
        case ImageFormat.TIFF:
          return MagickFormat.Tiff;
        case ImageFormat.JPEG:
          return MagickFormat.Jpeg;

        default:
          throw new InvalidDataException( $"Unsupported Image Format: {format}" );
      }
    }

  }

}
