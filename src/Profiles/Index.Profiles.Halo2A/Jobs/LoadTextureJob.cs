using DirectXTexNet;
using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Jobs;
using Index.Textures;
using LibSaber.Halo2A.Enumerations;
using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using Prism.Ioc;
using Serilog;

namespace Index.Profiles.Halo2A.Jobs
{

  public class LoadTextureJob : JobBase<DxgiTextureAsset>
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IDxgiTextureService _dxgiTextureService;

    private IAssetReference _assetReference;
    private Picture _picture;

    #endregion

    #region Constructor

    public LoadTextureJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      _assetManager = container.Resolve<IAssetManager>();
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
        SetStatus( "Deserializing Texture" );
        SetIndeterminate();

        var assetReference = Parameters.Get<IAssetReference>();
        _picture = DeserializePicture( assetReference );
      } );
    }

    protected override async Task OnExecuting()
    {
      var picture = _picture;
      var textureInfo = CreateTextureInfo( picture );

      SetSubStatus( "Preparing DXGI Texture" );
      var dxgiImage = _dxgiTextureService.CreateDxgiImageFromRawTextureData( picture.Data, textureInfo );

      SetSubStatus( "Generating Previews" );
      var previewStreams = _dxgiTextureService.CreateJpegImageStreams( dxgiImage, includeMips: false );

      var images = new List<ITextureAssetImage>();
      for ( var i = 0; i < previewStreams.Length; i++ )
      {
        var previewStream = previewStreams[ i ];
        var image = new TextureAssetImage( i, previewStream );
        images.Add( image );
      }

      var textureType = GetTextureType( _assetReference );
      var asset = new DxgiTextureAsset( _assetReference, textureType, images, dxgiImage );

      //var textureDefinition = await GetTextureDefinitionAsset();
      //if ( textureDefinition is null )
      //  Log.Warning( "Failed to find a texture definition for {textureName}.", asset.AssetName );
      //else
      //  asset.AdditionalData.Add( textureDefinition.AssetName, textureDefinition.TextStream );

      SetResult( asset );
    }

    #endregion

    #region Private Methods

    private Picture DeserializePicture( IAssetReference assetReference )
    {
      var stream = assetReference.Node.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      return Serializer.Deserialize<Picture>( reader );
    }

    private DxgiTextureInfo CreateTextureInfo( Picture picture )
    {
      return new DxgiTextureInfo
      {
        Width = picture.Width,
        Height = picture.Height,
        Depth = picture.Depth,
        FaceCount = picture.Faces,
        MipCount = picture.MipMapCount,
        Format = GetDxgiFormat( picture.Format )
      };
    }

    private DxgiTextureFormat GetDxgiFormat( PictureFormat format )
    {
      switch ( format )
      {
        case PictureFormat.A8R8G8B8:
          return DxgiTextureFormat.DXGI_FORMAT_B8G8R8A8_UNORM;
        case PictureFormat.A8L8:
          return DxgiTextureFormat.DXGI_FORMAT_R16G16B16A16_FLOAT;
        case PictureFormat.OXT1:
        case PictureFormat.AXT1:
          return DxgiTextureFormat.DXGI_FORMAT_BC1_UNORM;
        case PictureFormat.DXT3:
          return DxgiTextureFormat.DXGI_FORMAT_BC2_UNORM;
        case PictureFormat.DXT5:
          return DxgiTextureFormat.DXGI_FORMAT_BC3_UNORM;
        case PictureFormat.X8R8G8B8:
          return DxgiTextureFormat.DXGI_FORMAT_B8G8R8X8_UNORM;
        case PictureFormat.DXN:
          return DxgiTextureFormat.DXGI_FORMAT_BC5_UNORM;
        case PictureFormat.DXT5A:
          return DxgiTextureFormat.DXGI_FORMAT_BC4_UNORM;
        case PictureFormat.A16B16G16R16_F:
          return DxgiTextureFormat.DXGI_FORMAT_R16G16B16A16_FLOAT;
        case PictureFormat.R9G9B9E5_SHAREDEXP:
          return DxgiTextureFormat.DXGI_FORMAT_R9G9B9E5_SHAREDEXP;

        default:
          throw new NotSupportedException( "Invalid H2A Texture Type." );
      }
    }

    private static TextureType GetTextureType( IAssetReference assetReference )
    {
      var assetName = assetReference.AssetName;

      var suffixIndex = assetName.LastIndexOf( '_' );
      if ( suffixIndex == -1 )
        return TextureType.Diffuse;

      var suffix = assetName.Substring( suffixIndex + 1 );
      switch ( suffix )
      {
        case "nm":
          return TextureType.Normals;
        case "spec":
          return TextureType.SpecularColor;
        case "cube":
          return TextureType.Cubemap;
        case "lmdifdir":
        case "sm":
          return TextureType.Lightmap;

        case "akill":
        case "br":
        case "hdetm":
        case "mpmask":
          return TextureType.ChannelPacked;

        default:
          return TextureType.Diffuse;
      }

    }

    #endregion

  }

}
