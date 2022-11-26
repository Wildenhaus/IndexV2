using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Profiles.HaloCEA.FileSystem.Files;
using Index.Textures;

namespace Index.Profiles.HaloCEA.Assets
{

  public class CEATextureAssetFactory : IAssetFactory<DxgiTextureAsset>
  {

    private readonly IDxgiTextureService _dxgiTextureService;

    public CEATextureAssetFactory()
    {
      _dxgiTextureService = new DxgiTextureService();
    }

    public async Task<DxgiTextureAsset> LoadAsset( IAssetReference assetReference )
    {
      var assetNode = assetReference.Node as CEATextureFileNode;
      ASSERT_NOT_NULL( assetNode, "Node is not a CEATextureFileNode." );

      var textureInfo = CreateTextureInfo( assetNode );

      using var stream = assetNode.Open();
      var textureStream = _dxgiTextureService.CreateDDSStream( stream, textureInfo );
      var previewStreams = _dxgiTextureService.CreateJpegImageStreams( stream, textureInfo, includeMips: false );

      var images = new List<IDxgiTextureAssetImage>();
      foreach ( var imageStream in previewStreams )
      {
        var image = new DxgiTextureAssetImage( imageStream.ImageIndex, textureInfo )
        {
          PreviewStream = imageStream
        };

        images.Add( image );
      }

      var asset = new DxgiTextureAsset( assetReference );
      asset.Images = images;

      return asset;
    }

    private DxgiTextureInfo CreateTextureInfo( CEATextureFileNode node )
    {
      return new DxgiTextureInfo
      {
        Width = node.Width,
        Height = node.Height,
        Depth = node.Depth,
        FaceCount = node.FaceCount,
        MipCount = node.MipCount,
        Format = node.Format.ToDxgiFormat()
      };
    }

  }

}
