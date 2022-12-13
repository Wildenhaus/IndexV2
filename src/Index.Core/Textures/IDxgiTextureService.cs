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

    Stream CreateHDRImageStream( ScratchImage dxgiImage, int imageIndex = 0 );
    Stream[] CreateHDRImageStreams( ScratchImage dxgiImage, bool includeMips = false );

    Stream CreateJpegImageStream( ScratchImage dxgiImage, int imageIndex = 0, float quality = 1f );
    Stream[] CreateJpegImageStreams( ScratchImage dxgiImage, float quality = 1f, bool includeMips = false );

    Stream CreateTgaImageStream( ScratchImage dxgiImage, int imageIndex = 0 );
    Stream[] CreateTgaImageStreams( ScratchImage dxgiImage, bool includeMips = false );

    Stream CreateBmpImageStream( ScratchImage dxgiImage, int imageIndex = 0 );
    Stream[] CreateBmpImageStreams( ScratchImage dxgiImage, bool includeMips = false );

    Stream CreatePngImageStream( ScratchImage dxgiImage, int imageIndex = 0 );
    Stream[] CreatePngImageStreams( ScratchImage dxgiImage, bool includeMips = false );

    Stream CreateTiffImageStream( ScratchImage dxgiImage, int imageIndex = 0 );
    Stream[] CreateTiffImageStreams( ScratchImage dxgiImage, bool includeMips = false );

    #endregion

  }

}
