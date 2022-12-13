using System.Runtime.InteropServices;
using DirectXTexNet;

namespace Index.Textures
{

  public class DxgiTextureService : IDxgiTextureService
  {

    #region Constants

    private const DXGI_FORMAT DEFAULT_COLORSPACE = DXGI_FORMAT.R16G16B16A16_UNORM;

    public static readonly Guid WIC_CODEC_BMP = TexHelper.Instance.GetWICCodec( WICCodecs.BMP );
    public static readonly Guid WIC_CODEC_PNG = TexHelper.Instance.GetWICCodec( WICCodecs.PNG );
    public static readonly Guid WIC_CODEC_TIF = TexHelper.Instance.GetWICCodec( WICCodecs.TIFF );

    #endregion

    #region Public Methods

    public ScratchImage CreateDxgiImageFromRawTextureData( byte[] textureData, DxgiTextureInfo info )
    {
      var format = ( DXGI_FORMAT ) info.Format;

      // Create a scratch image
      ScratchImage img;
      if ( info.IsCubeMap )
        img = TexHelper.Instance.InitializeCube( format, info.Width, info.Height, info.FaceCount / 6, info.MipCount, CP_FLAGS.NONE );
      else
        img = TexHelper.Instance.Initialize2D( format, info.Width, info.Height, info.Depth, info.MipCount, CP_FLAGS.NONE );

      // Get a pointer to the scratch image's raw pixel data
      var srcData = textureData;
      var srcDataLen = textureData.Length;
      var pDest = img.GetPixels();
      var pDestLen = img.GetPixelsSize();
      ASSERT( pDestLen >= srcDataLen, "Source data will not fit in the destination image." );

      // Copy data into the scratch image
      Marshal.Copy( srcData, 0, pDest, srcDataLen );

      return img;
    }

    public ScratchImage CreateDxgiImageFromRawTextureData( Stream textureData, DxgiTextureInfo info )
      => CreateDxgiImageFromRawTextureData( textureData.CopyToArray(), info );

    public DxgiImageStream CreateDDSStream( byte[] textureData, DxgiTextureInfo textureInfo )
    {
      var dds = CreateDxgiImageFromRawTextureData( textureData, textureInfo );
      var ddsStream = dds.SaveToDDSMemory( DDS_FLAGS.NONE );

      dds.Dispose();
      return new DxgiImageStream( ddsStream, textureInfo );
    }

    public DxgiImageStream CreateDDSStream( ScratchImage dxgiImage, DxgiTextureInfo textureInfo )
    {
      var ddsStream = dxgiImage.SaveToDDSMemory( DDS_FLAGS.NONE );
      return new DxgiImageStream( ddsStream, textureInfo );
    }

    public Stream CreateHDRImageStream( ScratchImage dxgiImage, int imageIndex = 0 )
      => CreateSingleRgbImageStream( dxgiImage, DEFAULT_COLORSPACE, imageIndex, ( dds, index ) => dds.SaveToHDRMemory( index ) );

    public Stream[] CreateHDRImageStreams( ScratchImage dxgiImage, bool includeMips = false )
      => CreateRgbImageStreams( dxgiImage, DEFAULT_COLORSPACE, includeMips, ( dds, index ) => dds.SaveToHDRMemory( index ) );

    public Stream CreateJpegImageStream( ScratchImage dxgiImage, int imageIndex = 0, float quality = 1f )
      => CreateSingleRgbImageStream( dxgiImage, DEFAULT_COLORSPACE, imageIndex, ( dds, index ) => dds.SaveToJPGMemory( index, quality ) );

    public Stream[] CreateJpegImageStreams( ScratchImage dxgiImage, float quality = 1f, bool includeMips = false )
      => CreateRgbImageStreams( dxgiImage, DEFAULT_COLORSPACE, includeMips, ( dds, index ) => dds.SaveToJPGMemory( index, quality ) );

    public Stream CreateTgaImageStream( ScratchImage dxgiImage, int imageIndex = 0 )
      => CreateSingleRgbImageStream( dxgiImage, DEFAULT_COLORSPACE, imageIndex, ( dds, index ) => dds.SaveToTGAMemory( index ) );

    public Stream[] CreateTgaImageStreams( ScratchImage dxgiImage, bool includeMips = false )
      => CreateRgbImageStreams( dxgiImage, DEFAULT_COLORSPACE, includeMips, ( dds, index ) => dds.SaveToTGAMemory( index ) );

    public Stream CreateBmpImageStream( ScratchImage dxgiImage, int imageIndex = 0 )
      => CreateSingleRgbImageStream( dxgiImage, DEFAULT_COLORSPACE, imageIndex, ( dds, index ) => dds.SaveToWICMemory( index, WIC_FLAGS.NONE, WIC_CODEC_BMP ) );

    public Stream[] CreateBmpImageStreams( ScratchImage dxgiImage, bool includeMips = false )
      => CreateRgbImageStreams( dxgiImage, DEFAULT_COLORSPACE, includeMips, ( dds, index ) => dds.SaveToWICMemory( index, WIC_FLAGS.NONE, WIC_CODEC_BMP ) );

    public Stream CreatePngImageStream( ScratchImage dxgiImage, int imageIndex = 0 )
      => CreateSingleRgbImageStream( dxgiImage, DEFAULT_COLORSPACE, imageIndex, ( dds, index ) => dds.SaveToWICMemory( index, WIC_FLAGS.NONE, WIC_CODEC_PNG ) );

    public Stream[] CreatePngImageStreams( ScratchImage dxgiImage, bool includeMips = false )
     => CreateRgbImageStreams( dxgiImage, DEFAULT_COLORSPACE, includeMips, ( dds, index ) => dds.SaveToWICMemory( index, WIC_FLAGS.NONE, WIC_CODEC_PNG ) );

    public Stream CreateTiffImageStream( ScratchImage dxgiImage, int imageIndex = 0 )
      => CreateSingleRgbImageStream( dxgiImage, DEFAULT_COLORSPACE, imageIndex, ( dds, index ) => dds.SaveToWICMemory( index, WIC_FLAGS.NONE, WIC_CODEC_TIF ) );

    public Stream[] CreateTiffImageStreams( ScratchImage dxgiImage, bool includeMips = false )
     => CreateRgbImageStreams( dxgiImage, DEFAULT_COLORSPACE, includeMips, ( dds, index ) => dds.SaveToWICMemory( index, WIC_FLAGS.NONE, WIC_CODEC_TIF ) );

    #endregion

    #region Private Methods

    private TStream CreateSingleRgbImageStream<TStream>(
      ScratchImage dxgiImage,
      DXGI_FORMAT destFormat,
      int imageIndex,
      Func<ScratchImage, int, TStream> createStreamFunc )
      where TStream : Stream
    {
      var conversionRequired = CoerceDDSImageToRgbColorspace( dxgiImage, destFormat, out var argbImage );

      var stream = createStreamFunc( argbImage, imageIndex );

      if ( conversionRequired )
        argbImage.Dispose();

      return stream;
    }

    private TStream[] CreateRgbImageStreams<TStream>(
      ScratchImage dxgiImage,
      DXGI_FORMAT destFormat,
      bool includeMips,
      Func<ScratchImage, int, TStream> createStreamFunc )
      where TStream : Stream
    {
      var imageCount = dxgiImage.GetImageCount();
      var mipLevels = dxgiImage.GetMetadata().MipLevels;

      var conversionRequired = CoerceDDSImageToRgbColorspace( dxgiImage, destFormat, out var argbImage );

      var streams = new List<TStream>();
      for ( var imageIdx = 0; imageIdx < imageCount; imageIdx++ )
      {
        var isMipMap = imageIdx % mipLevels != 0;
        if ( isMipMap && !includeMips )
          continue;

        var stream = createStreamFunc( argbImage, imageIdx );
        streams.Add( stream );
      }

      if ( conversionRequired )
        argbImage.Dispose();

      return streams.ToArray();
    }

    /// <summary>
    ///   Prepares the source DDS image to an RGB colorspace so that it can be exported to a non-DDS format.
    /// </summary>
    /// <param name="sourceImage">
    ///   The source DDS image.
    /// </param>
    /// <param name="rgbImage">
    ///   The converted image. 
    ///   NOTE: This will be the same object reference as the source image if no conversion was required.
    /// </param>
    /// <returns>
    ///   A bool denoting whether or not the source DDS image required any conversion or decompression.
    /// </returns>
    private bool CoerceDDSImageToRgbColorspace( ScratchImage sourceImage, DXGI_FORMAT destFormat, out ScratchImage rgbImage )
    {
      rgbImage = sourceImage;
      var format = sourceImage.GetMetadata().Format;

      if ( format == destFormat )
        return false;

      // If compressed with BCx compression
      if ( format.ToString().StartsWith( "BC" ) )
        rgbImage = sourceImage.Decompress( destFormat );
      else
        rgbImage = sourceImage.Convert( destFormat, TEX_FILTER_FLAGS.DEFAULT, 0 );

      return true;
    }

    #endregion

  }

}
