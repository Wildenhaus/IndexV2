using System.Runtime.InteropServices;
using DirectXTexNet;

namespace Index.Textures
{

  public class DxgiTextureService : IDxgiTextureService
  {

    #region Constants

    private const DXGI_FORMAT DEFAULT_RGB_CONVERSION_FORMAT = DXGI_FORMAT.R16G16B16A16_UNORM;

    #endregion

    public Stream CreateDDSStream( byte[] textureData, DxgiTextureInfo textureInfo )
    {
      var dds = CreateDDSFromRawTextureData( textureData, textureInfo );
      return dds.SaveToDDSMemory( DDS_FLAGS.NONE );
    }

    public DxgiImageStream[] CreateHDRImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, bool includeMips = false )
      => CreateRgbImageStreams( textureData, textureInfo, includeMips, ( dds, index ) => dds.SaveToHDRMemory( index ) );

    public DxgiImageStream CreateSingleHDRImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 )
      => CreateSingleRgbImageStream( textureData, textureInfo, imageIndex, ( dds, index ) => dds.SaveToHDRMemory( index ) );

    public DxgiImageStream[] CreateJpegImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, float quality = 1f, bool includeMips = false )
      => CreateRgbImageStreams( textureData, textureInfo, includeMips, ( dds, index ) => dds.SaveToJPGMemory( index, quality ) );

    public DxgiImageStream CreateSingleJpegImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0, float quality = 1f )
      => CreateSingleRgbImageStream( textureData, textureInfo, imageIndex, ( dds, index ) => dds.SaveToJPGMemory( index, quality ) );

    public DxgiImageStream[] CreateTgaImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, bool includeMips = false )
      => CreateRgbImageStreams( textureData, textureInfo, includeMips, ( dds, index ) => dds.SaveToTGAMemory( index ) );

    public DxgiImageStream CreateSingleTgaImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 )
      => CreateSingleRgbImageStream( textureData, textureInfo, imageIndex, ( dds, index ) => dds.SaveToTGAMemory( index ) );

    #region Private Methods

    private ScratchImage CreateDDSFromRawTextureData( byte[] textureData, DxgiTextureInfo info )
    {
      var format = ( DXGI_FORMAT ) info.Format;

      // Create a scratch image
      ScratchImage img;
      if ( info.IsCubeMap )
        img = TexHelper.Instance.InitializeCube( format, info.Width, info.Height, info.FaceCount, info.MipCount, CP_FLAGS.NONE );
      else
        img = TexHelper.Instance.Initialize2D( format, info.Width, info.Height, 1, info.MipCount, CP_FLAGS.NONE );

      // Get a pointer to the scratch image's raw pixel data
      var srcData = textureData;
      var srcDataLen = textureData.Length;
      var pDest = img.GetPixels();
      var pDestLen = img.GetPixelsSize();
      ASSERT( pDestLen == srcDataLen, "Source data will not fit in the destination image." );

      // Copy data into the scratch image
      Marshal.Copy( srcData, 0, pDest, srcDataLen );

      return img;
    }

    private ScratchImage CreateRgbDDSFromRawTextureFormat( byte[] textureData, DxgiTextureInfo info )
    {
      var dds = CreateDDSFromRawTextureData( textureData, info );
      if ( CoerceDDSImageToRgbColorspace( dds, out var rgbImage ) )
        dds?.Dispose();

      return rgbImage;
    }

    private DxgiImageStream CreateSingleRgbImageStream( byte[] textureData, DxgiTextureInfo info, int imageIndex, Func<ScratchImage, int, Stream> createStreamFunc )
    {
      var dds = CreateRgbDDSFromRawTextureFormat( textureData, info );
      var stream = createStreamFunc( dds, 0 );
      return new DxgiImageStream( stream, imageIndex, info );
    }

    private DxgiImageStream[] CreateRgbImageStreams( byte[] textureData, DxgiTextureInfo info, bool includeMips, Func<ScratchImage, int, Stream> createStreamFunc )
    {
      var dds = CreateRgbDDSFromRawTextureFormat( textureData, info );
      var imageCount = dds.GetImageCount();

      var streams = new List<DxgiImageStream>();
      for ( var imageIdx = 0; imageIdx < imageCount; imageIdx++ )
      {
        var isMipMap = imageIdx % info.MipCount != 0;
        if ( isMipMap && !includeMips )
          continue;

        var stream = createStreamFunc( dds, imageIdx );
        streams.Add( new DxgiImageStream( stream, imageIdx, info ) );
      }

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

      if ( format.ToString().StartsWith( "BC" ) )
        rgbImage = sourceImage.Decompress( DEFAULT_RGB_CONVERSION_FORMAT );
      else
        rgbImage = sourceImage.Convert( DEFAULT_RGB_CONVERSION_FORMAT, TEX_FILTER_FLAGS.DEFAULT, 0 );

      return true;
    }

    #endregion

  }

}
