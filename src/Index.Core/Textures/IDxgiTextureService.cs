namespace Index.Textures
{

  public interface IDxgiTextureService
  {

    #region Public Methods

    DxgiImageStream CreateDDSStream( byte[] textureData, DxgiTextureInfo textureInfo );
    DxgiImageStream CreateDDSStream( Stream textureData, DxgiTextureInfo textureInfo ) => CreateDDSStream( textureData.CopyToArray(), textureInfo );

    Stream[] CreateHDRImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, bool includeMips = false );
    Stream[] CreateHDRImageStreams( Stream textureData, DxgiTextureInfo textureInfo, bool includeMips = false ) => CreateHDRImageStreams( textureData.CopyToArray(), textureInfo, includeMips );
    Stream CreateSingleHDRImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 );
    Stream CreateSingleHDRImageStream( Stream textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 ) => CreateSingleHDRImageStream( textureData.CopyToArray(), textureInfo, imageIndex );

    Stream[] CreateJpegImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, float quality = 1f, bool includeMips = false );
    Stream[] CreateJpegImageStreams( Stream textureData, DxgiTextureInfo textureInfo, float quality = 1f, bool includeMips = false ) => CreateJpegImageStreams( textureData.CopyToArray(), textureInfo, quality, includeMips );
    Stream CreateSingleJpegImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0, float quality = 1f );
    Stream CreateSingleJpegImageStream( Stream textureData, DxgiTextureInfo textureInfo, int imageIndex = 0, float quality = 1f ) => CreateSingleJpegImageStream( textureData.CopyToArray(), textureInfo, imageIndex, quality );

    Stream[] CreateTgaImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, bool includeMips = false );
    Stream[] CreateTgaImageStreams( Stream textureData, DxgiTextureInfo textureInfo, bool includeMips = false ) => CreateTgaImageStreams( textureData.CopyToArray(), textureInfo, includeMips );
    Stream CreateSingleTgaImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 );
    Stream CreateSingleTgaImageStream( Stream textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 ) => CreateSingleTgaImageStream( textureData.CopyToArray(), textureInfo, imageIndex );

    #endregion

  }

}
