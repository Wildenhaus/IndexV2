using DirectXTexNet;

namespace Index.Textures
{

  public interface IDxgiTextureService
  {

    #region Public Methods

    ScratchImage CreateDxgiImageFromRawTextureData( byte[] textureData, DxgiTextureInfo info );
    ScratchImage CreateDxgiImageFromRawTextureData( Stream textureData, DxgiTextureInfo info );

    DxgiImageStream CreateDDSStream( byte[] textureData, DxgiTextureInfo textureInfo );
    DxgiImageStream CreateDDSStream( ScratchImage dxgiImage, DxgiTextureInfo textureInfo );

    Stream[] CreateHDRImageStreams( ScratchImage dxgiImage, bool includeMips = false );
    Stream CreateSingleHDRImageStream( ScratchImage dxgiImage, int imageIndex = 0 );

    Stream[] CreateJpegImageStreams( ScratchImage dxgiImage, float quality = 1f, bool includeMips = false );
    Stream CreateSingleJpegImageStream( ScratchImage dxgiImage, int imageIndex = 0, float quality = 1f );

    Stream[] CreateTgaImageStreams( ScratchImage dxgiImage, bool includeMips = false );
    Stream CreateSingleTgaImageStream( ScratchImage dxgiImage, int imageIndex = 0 );

    #endregion

  }

}
