using System.Runtime.InteropServices;
using DirectXTexNet;

namespace Index.Textures
{

  public class DxgiTextureService : IDxgiTextureService
  {

    #region Constants

    private const DXGI_FORMAT DEFAULT_RGB_CONVERSION_FORMAT = DXGI_FORMAT.R16G16B16A16_UNORM;

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

    public Stream[] CreateHDRImageStreams( ScratchImage dxgiImage, bool includeMips = false )
      => CreateRgbImageStreams( dxgiImage, includeMips, ( dds, index ) => dds.SaveToHDRMemory( index ) );

    public Stream CreateSingleHDRImageStream( ScratchImage dxgiImage, int imageIndex = 0 )
      => CreateSingleRgbImageStream( dxgiImage, imageIndex, ( dds, index ) => dds.SaveToHDRMemory( index ) );

    public Stream[] CreateJpegImageStreams( ScratchImage dxgiImage, float quality = 1f, bool includeMips = false )
      => CreateRgbImageStreams( dxgiImage, includeMips, ( dds, index ) => dds.SaveToJPGMemory( index, quality ) );

    public Stream CreateSingleJpegImageStream( ScratchImage dxgiImage, int imageIndex = 0, float quality = 1f )
      => CreateSingleRgbImageStream( dxgiImage, imageIndex, ( dds, index ) => dds.SaveToJPGMemory( index, quality ) );

    public Stream[] CreateTgaImageStreams( ScratchImage dxgiImage, bool includeMips = false )
      => CreateRgbImageStreams( dxgiImage, includeMips, ( dds, index ) => dds.SaveToTGAMemory( index ) );

    public Stream CreateSingleTgaImageStream( ScratchImage dxgiImage, int imageIndex = 0 )
      => CreateSingleRgbImageStream( dxgiImage, imageIndex, ( dds, index ) => dds.SaveToTGAMemory( index ) );

    #endregion

    #region Private Methods

    private Stream CreateSingleRgbImageStream( ScratchImage dxgiImage, int imageIndex, Func<ScratchImage, int, Stream> createStreamFunc )
    {
      var conversionRequired = CoerceDDSImageToRgbColorspace( dxgiImage, out var argbImage );

      var stream = createStreamFunc( argbImage, imageIndex );

      if ( conversionRequired )
        argbImage.Dispose();

      return stream;
    }

    private Stream[] CreateRgbImageStreams( ScratchImage dxgiImage, bool includeMips, Func<ScratchImage, int, Stream> createStreamFunc )
    {
      var imageCount = dxgiImage.GetImageCount();
      var mipLevels = dxgiImage.GetMetadata().MipLevels;

      var conversionRequired = CoerceDDSImageToRgbColorspace( dxgiImage, out var argbImage );

      var streams = new List<Stream>();
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
    private bool CoerceDDSImageToRgbColorspace( ScratchImage sourceImage, out ScratchImage rgbImage )
    {
      rgbImage = sourceImage;
      var format = sourceImage.GetMetadata().Format;

      if ( format == DEFAULT_RGB_CONVERSION_FORMAT )
        return false;

      // If compressed with BCx compression
      if ( format.ToString().StartsWith( "BC" ) )
        rgbImage = sourceImage.Decompress( DEFAULT_RGB_CONVERSION_FORMAT );
      else
        rgbImage = sourceImage.Convert( DEFAULT_RGB_CONVERSION_FORMAT, TEX_FILTER_FLAGS.DEFAULT, 0 );

      return true;
    }

    #endregion

  }

}
