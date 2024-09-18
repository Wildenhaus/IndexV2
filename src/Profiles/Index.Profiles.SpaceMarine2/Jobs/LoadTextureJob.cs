using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Assets.Textures;
using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Jobs;
using Index.Textures;
using Prism.Ioc;
using Serilog;
using LibSaber.SpaceMarine2.Structures.Resources;
using LibSaber.IO;
using LibSaber.SpaceMarine2.Serialization;
using LibSaber.SpaceMarine2.Enumerations;
using Index.Domain.FileSystem;

namespace Index.Profiles.SpaceMarine2.Jobs
{

  public class LoadTextureJob : JobBase<DxgiTextureAsset>
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IDxgiTextureService _dxgiTextureService;
    private readonly IFileSystem _fileSystem;

    private IAssetReference _assetReference;
    private PctResource _resource;

    #endregion

    #region Constructor

    public LoadTextureJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      _assetManager = container.Resolve<IAssetManager>();
      _dxgiTextureService = container.Resolve<IDxgiTextureService>();
      _fileSystem = container.Resolve<IFileSystem>();

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
        _resource = DeserializePctResource( assetReference );
      } );
    }

    protected override async Task OnExecuting()
    {
      var data = GetTextureData();
      var textureInfo = CreateTextureInfo( _resource );

      SetSubStatus( "Preparing DXGI Texture" );
      var dxgiImage = _dxgiTextureService.CreateDxgiImageFromRawTextureData( data, textureInfo );

      SetSubStatus( "Generating Previews" );
      var previewStreams = _dxgiTextureService.CreateJpegImageStreams( dxgiImage, includeMips: false );

      var images = new List<ITextureAssetImage>();
      for ( var i = 0; i < previewStreams.Length; i++ )
      {
        var previewStream = previewStreams[ i ];
        var image = new TextureAssetImage( i, previewStream );
        images.Add( image );
      }

      // var textureType = GetTextureType( _assetReference );
      var asset = new DxgiTextureAsset( _assetReference, TextureType.Unknown, images, dxgiImage );

      //var textureDefinition = await GetTextureDefinitionAsset();
      //if ( textureDefinition is null )
      //  Log.Warning( "Failed to find a texture definition for {textureName}.", asset.AssetName );
      //else
      //  asset.AdditionalData.Add( textureDefinition.AssetName, textureDefinition.TextStream );

      SetResult( asset );
    }

    #endregion

    #region Private Methods

    private PctResource DeserializePctResource( IAssetReference assetReference )
    {
      var stream = assetReference.Node.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      return Serializer<PctResource>.Deserialize( reader );
    }

    private byte[] GetTextureData()
    {
      using var ms = new MemoryStream();

      foreach(var mipName in _resource.mipMaps)
      {
        var node = _fileSystem.EnumerateFiles().FirstOrDefault(x => x.Name.Contains(mipName));
        if ( node is null )
          continue;
        using var mipStream = node.Open();
        mipStream.CopyTo( ms );
      }

      ms.Position = 0;
      return ms.ToArray();
    }

    private DxgiTextureInfo CreateTextureInfo( PctResource resource )
    {
      return new DxgiTextureInfo
      {
        Width = resource.header.sx,
        Height = resource.header.sy,
        Depth = resource.header.sz,
        FaceCount = resource.header.nFaces,
        MipCount = resource.header.nMipMap,
        Format = GetDxgiFormat( resource.TextureFormat )
      };
    }

    private DxgiTextureFormat GetDxgiFormat( SM2TextureFormat format )
    {
      switch ( format )
      {
        case SM2TextureFormat.ARGB8888:
          return DxgiTextureFormat.DXGI_FORMAT_B8G8R8A8_UNORM;
        case SM2TextureFormat.RGBA16161616F:
          return DxgiTextureFormat.DXGI_FORMAT_R16G16B16A16_FLOAT;
        case SM2TextureFormat.OXT1:
        case SM2TextureFormat.AXT1:
          return DxgiTextureFormat.DXGI_FORMAT_BC1_UNORM;
        case SM2TextureFormat.XRGB8888:
          return DxgiTextureFormat.DXGI_FORMAT_B8G8R8X8_UNORM;
        case SM2TextureFormat.DXN:
          return DxgiTextureFormat.DXGI_FORMAT_BC5_UNORM;
        case SM2TextureFormat.DXT5A:
          return DxgiTextureFormat.DXGI_FORMAT_BC4_UNORM;
        case SM2TextureFormat.R9G9B9E5_SHAREDEXP:
          return DxgiTextureFormat.DXGI_FORMAT_R9G9B9E5_SHAREDEXP;
        case SM2TextureFormat.BC7:
          return DxgiTextureFormat.DXGI_FORMAT_BC7_UNORM;
        case SM2TextureFormat.BC7A:
          return DxgiTextureFormat.DXGI_FORMAT_BC7_UNORM;
        case SM2TextureFormat.R8U:
          return DxgiTextureFormat.DXGI_FORMAT_R8_UNORM;

        default:
          throw new NotSupportedException( "Invalid SM2 Texture Type." );
      }
    }

    #endregion

  }

}
