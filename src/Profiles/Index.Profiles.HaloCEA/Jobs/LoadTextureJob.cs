using Index.Core.IO;
using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Jobs;
using Index.Profiles.HaloCEA.FileSystem.Files;
using Index.Textures;
using Prism.Ioc;

namespace Index.Profiles.HaloCEA.Jobs
{

  public class LoadTextureJob : JobBase<DxgiTextureAsset>
  {

    #region Data Members

    private readonly IDxgiTextureService _dxgiTextureService;

    private IAssetReference _assetReference;
    private CEAPictureData _pictureData;

    #endregion

    #region Constructor

    public LoadTextureJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      _dxgiTextureService = container.Resolve<IDxgiTextureService>();

      _assetReference = Parameters.Get<IAssetReference>();
      Name = $"Loading {_assetReference.AssetName}";
    }

    #endregion

    #region Overrides

    protected override Task OnInitializing()
    {
      return Task.Run( () =>
      {
        SetStatus( "Reading Texture Metadata" );
        SetIndeterminate();

        var assetReference = Parameters.Get<IAssetReference>();
        _pictureData = ReadPictureData( assetReference );
      } );
    }

    protected override Task OnExecuting()
    {
      return Task.Run( () =>
      {
        var stream = _pictureData.DataStream;
        var textureInfo = CreateTextureInfo( _pictureData );

        SetStatus( "Preparing DXGI Texture" );
        var textureStream = _dxgiTextureService.CreateDDSStream( stream, textureInfo );

        SetStatus( "Generating Previews" );
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

        var asset = new DxgiTextureAsset( _assetReference );
        asset.Images = images;

        SetResult( asset );
      } );
    }

    #endregion

    #region Private Methods

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

    #endregion

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
