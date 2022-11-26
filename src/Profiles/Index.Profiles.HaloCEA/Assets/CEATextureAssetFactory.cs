using Index.Core.IO;
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
      var pictureData = ReadPictureData( assetReference );
      var textureInfo = CreateTextureInfo( pictureData );

      var textureStream = _dxgiTextureService.CreateDDSStream( pictureData.DataStream, textureInfo );
      var previewStreams = _dxgiTextureService.CreateJpegImageStreams( pictureData.DataStream, textureInfo, includeMips: false );

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

    private DxgiTextureInfo CreateTextureInfo( CEAPictureData pict )
    {
      return new DxgiTextureInfo
      {
        Width = pict.Width,
        Height = pict.Height,
        Depth = pict.Depth,
        FaceCount = pict.FaceCount,
        MipCount = pict.MipCount,
        Format = pict.Format.ToDxgiFormat()
      };
    }

    private CEAPictureData ReadPictureData( IAssetReference assetReference )
    {
      const int MAGIC_PICT = 0x50494354; //PICT

      var node = assetReference.Node;

      var data = new CEAPictureData();

      var stream = node.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      // Sentinel F0 - PICT Magic
      ASSERT( reader.ReadInt16() == 0xF0 );
      _ = reader.ReadInt32();
      ASSERT( reader.ReadInt32() == MAGIC_PICT, "Invalid PICT Magic." );

      // Sentinel 0102 - Dimensions
      ASSERT( reader.ReadInt16() == 0x0102 );
      _ = reader.ReadInt32();
      data.Width = reader.ReadInt32();
      data.Height = reader.ReadInt32();
      data.Depth = reader.ReadInt32();
      data.FaceCount = reader.ReadInt32();

      // Sentinel F2 - Unknown
      ASSERT( reader.ReadInt16() == 0xF2 );
      _ = reader.ReadInt32();
      data.UNK_F2 = reader.ReadInt32();

      // Sentinel F9 - Mip Count
      ASSERT( reader.ReadInt16() == 0xF9 );
      _ = reader.ReadInt32();
      data.MipCount = reader.ReadInt32();

      // Sentinel 0xFF - Raw Data
      ASSERT( reader.ReadInt16() == 0xFF );
      _ = reader.ReadInt32();

      if ( node is CEATextureFileNode textureNode )
      {
        data.Width = textureNode.Width;
        data.Height = textureNode.Height;
        data.Depth = textureNode.Depth;
        data.FaceCount = textureNode.FaceCount;
        data.MipCount = textureNode.MipCount;
        data.Format = textureNode.Format;
      }

      var sizeOfData = stream.Length - stream.Position;
      data.DataStream = new StreamSegment( stream, reader.Position, sizeOfData );

      return data;
    }

    #region Embedded Types

    internal class CEAPictureData
    {
      public int Width;
      public int Height;
      public int Depth;
      public int FaceCount;
      public int MipCount;
      public int UNK_F2;
      public CEATextureFormat Format;
      public Stream DataStream;
    }

    #endregion

  }

}
