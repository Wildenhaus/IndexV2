namespace Index.Textures
{

  public interface IDxgiTextureService
  {

    #region Public Methods

    Stream CreateDDSStream( byte[] textureData, DxgiTextureInfo textureInfo );
    Stream CreateDDSStream( Stream textureData, DxgiTextureInfo textureInfo ) => CreateDDSStream( textureData.CopyToArray(), textureInfo );

    DxgiImageStream[] CreateHDRImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, bool includeMips = false );
    DxgiImageStream[] CreateHDRImageStreams( Stream textureData, DxgiTextureInfo textureInfo, bool includeMips = false ) => CreateHDRImageStreams( textureData.CopyToArray(), textureInfo, includeMips );
    DxgiImageStream CreateSingleHDRImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 );
    DxgiImageStream CreateSingleHDRImageStream( Stream textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 ) => CreateSingleHDRImageStream( textureData.CopyToArray(), textureInfo, imageIndex );

    DxgiImageStream[] CreateJpegImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, float quality = 1f, bool includeMips = false );
    DxgiImageStream[] CreateJpegImageStreams( Stream textureData, DxgiTextureInfo textureInfo, float quality = 1f, bool includeMips = false ) => CreateJpegImageStreams( textureData.CopyToArray(), textureInfo, quality, includeMips );
    DxgiImageStream CreateSingleJpegImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0, float quality = 1f );
    DxgiImageStream CreateSingleJpegImageStream( Stream textureData, DxgiTextureInfo textureInfo, int imageIndex = 0, float quality = 1f ) => CreateSingleJpegImageStream( textureData.CopyToArray(), textureInfo, imageIndex, quality );

    DxgiImageStream[] CreateTgaImageStreams( byte[] textureData, DxgiTextureInfo textureInfo, bool includeMips = false );
    DxgiImageStream[] CreateTgaImageStreams( Stream textureData, DxgiTextureInfo textureInfo, bool includeMips = false ) => CreateTgaImageStreams( textureData.CopyToArray(), textureInfo, includeMips );
    DxgiImageStream CreateSingleTgaImageStream( byte[] textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 );
    DxgiImageStream CreateSingleTgaImageStream( Stream textureData, DxgiTextureInfo textureInfo, int imageIndex = 0 ) => CreateSingleTgaImageStream( textureData.CopyToArray(), textureInfo, imageIndex );

    #endregion

  }

}
