using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Domain.FileSystem;
using Index.Jobs;
using Index.Profiles.SpaceMarine2.Assets;
using Index.Textures;
using LibSaber.IO;
using LibSaber.SpaceMarine2.Enumerations;
using LibSaber.SpaceMarine2.Serialization;
using LibSaber.SpaceMarine2.Structures;
using LibSaber.SpaceMarine2.Structures.Resources;
using Prism.Ioc;
using Serilog;

namespace Index.Profiles.SpaceMarine2.Jobs
{

  public class LoadTextureJob : JobBase<DxgiTextureAsset>
  {

    #region Data Members

    private readonly IAssetManager _assetManager;
    private readonly IDxgiTextureService _dxgiTextureService;
    private readonly IFileSystem _fileSystem;

    private IAssetReference _assetReference;
    private resDESC_PCT _resource;

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

      var textureType = GetTextureType();
      var asset = new DxgiTextureAsset( _assetReference, textureType, dxgiImage );

      var textureDefinition = await GetTextureDefinitionAsset();
      if ( textureDefinition is null )
      {
        //Log.Warning( "Failed to find a texture definition for {textureName}.", asset.AssetName );
      }
      else
        asset.AdditionalData.Add( textureDefinition.AssetName, textureDefinition.TextStream );

      SetResult( asset );
    }

    #endregion

    #region Private Methods

    private resDESC_PCT DeserializePctResource( IAssetReference assetReference )
    {
      var stream = assetReference.Node.Open();
      var reader = new NativeReader( stream, Endianness.LittleEndian );

      return Serializer<resDESC_PCT>.Deserialize( reader );
    }

    private byte[] GetTextureData()
    {
      if ( !string.IsNullOrWhiteSpace( _resource.pct ) )
        return GetTextureDataOldFormat();
      else
        return GetTextureDataNewFormat();
    }

    private byte[] GetTextureDataOldFormat()
    {
      var pctFileName = _resource.pct;

      var node = _fileSystem.EnumerateFiles().FirstOrDefault( x => Path.GetFileName(x.Name) == pctFileName );
      if ( node is null )
        throw new Exception( $"Texture specifies old format, but PCT file not found: {pctFileName}" );

      var reader = new NativeReader( node.Open(), Endianness.LittleEndian );
      var pct = Serializer<pctPICTURE>.Deserialize( reader );

      _resource.header.nMipMap = pct.MipMapCount;
      _resource.header.nFaces = pct.Faces;
      _resource.header.sx = pct.Width;
      _resource.header.sy = pct.Height;
      _resource.header.sz = pct.Depth;

      return pct.Data;
    }

    private byte[] GetTextureDataNewFormat()
    {
      using var ms = new MemoryStream();

      foreach ( var mipName in _resource.mipMaps )
      {
        var node = _fileSystem.EnumerateFiles().FirstOrDefault( x => x.Name.Contains( mipName ) );
        if ( node is null )
          continue;
        using var mipStream = node.Open();
        mipStream.CopyTo( ms );
      }

      ms.Position = 0;
      return ms.ToArray();
    }

    private DxgiTextureInfo CreateTextureInfo( resDESC_PCT resource )
    {
      // TODO: We're ignoring cubemaps

      return new DxgiTextureInfo
      {
        Width = resource.header.sx,
        Height = resource.header.sy,
        Depth = resource.header.nFaces * resource.header.sz,
        FaceCount = 1,
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
        case SM2TextureFormat.ARGB16161616U:
          return DxgiTextureFormat.DXGI_FORMAT_R16G16B16A16_UNORM;
        case SM2TextureFormat.BC6U:
          return DxgiTextureFormat.DXGI_FORMAT_BC6H_UF16;
        case SM2TextureFormat.BC7:
        case SM2TextureFormat.BC7A:
          return DxgiTextureFormat.DXGI_FORMAT_BC7_UNORM;
        case SM2TextureFormat.DXN:
          return DxgiTextureFormat.DXGI_FORMAT_BC5_UNORM;
        case SM2TextureFormat.DXT5A:
          return DxgiTextureFormat.DXGI_FORMAT_BC4_UNORM;
        case SM2TextureFormat.AXT1:
        case SM2TextureFormat.OXT1:
          return DxgiTextureFormat.DXGI_FORMAT_BC1_UNORM;
        case SM2TextureFormat.R8U:
          return DxgiTextureFormat.DXGI_FORMAT_R8_UNORM;
        case SM2TextureFormat.R16:
          return DxgiTextureFormat.DXGI_FORMAT_R16_SNORM;
        case SM2TextureFormat.R16G16:
          return DxgiTextureFormat.DXGI_FORMAT_R16G16_SINT;
        case SM2TextureFormat.RGBA16161616F:
          return DxgiTextureFormat.DXGI_FORMAT_R16G16B16A16_FLOAT;
        case SM2TextureFormat.XT5:
          return DxgiTextureFormat.DXGI_FORMAT_BC3_UNORM;
        case SM2TextureFormat.XRGB8888:
          return DxgiTextureFormat.DXGI_FORMAT_B8G8R8X8_UNORM;

        default:
          throw new NotSupportedException( "Invalid SM2 Texture Type: " + format.ToString() );
      }
    }

    private TextureType GetTextureType()
    {
      var type = _resource.texType;
      if ( type is null )
        return TextureType.Unknown;

      switch ( _resource.texType )
      {
        case "nm":
        case "det":
          return TextureType.Normals;
        case "em":
          return TextureType.Emission;
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

    private async Task<SM2TextureDefinitionAsset> GetTextureDefinitionAsset()
    {
      var textureDefinitionName = Path.ChangeExtension( _assetReference.AssetName, ".td" );
      textureDefinitionName = Path.GetFileName( textureDefinitionName );

      _assetManager.TryGetAssetReference( typeof( SM2TextureDefinitionAsset ),
        textureDefinitionName, out var textureDefinitionAssetReference );

      if ( textureDefinitionAssetReference is null )
        return null;

      var textureDefinition = await _assetManager.LoadAssetAsync<SM2TextureDefinitionAsset>( textureDefinitionAssetReference );
      return textureDefinition;
    }

    #endregion

  }

}
