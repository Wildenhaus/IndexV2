using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Jobs;
using Index.Textures;
using Prism.Ioc;

namespace Index.Domain.Assets.Textures.Dxgi
{

  public class DxgiTextureAssetExportJob : AssetExportJobBase<DxgiTextureAsset, TextureExportOptions>
  {

    #region Data Members

    private readonly IDxgiTextureService _dxgiTextureService;

    #endregion

    #region Constructor

    public DxgiTextureAssetExportJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      _dxgiTextureService = container.Resolve<IDxgiTextureService>();
    }

    #endregion

    #region Overrides

    protected override async Task ExportAsset()
    {
      // TODO: Normal Map Processing
      var exportStream = GetExportStream();

      SetStatus( "Writing File" );
      await WriteFile( exportStream );
    }

    #endregion

    #region Private Methods

    private string GetExportFilePath()
    {
      var exportPath = Options.ExportPath;
      var fileExt = Options.ExportFormat.GetFileExtension();

      exportPath = Path.Combine( exportPath, Asset.AssetName + fileExt );
      return exportPath;
    }

    private Stream GetExportStream()
    {
      var dxgiImage = Asset.DxgiImage;
      switch ( Options.ExportFormat )
      {
        case TextureExportFormat.DDS:
          return _dxgiTextureService.CreateDDSStream( dxgiImage );
        case TextureExportFormat.TGA:
          return _dxgiTextureService.CreateTgaImageStream( dxgiImage );
        case TextureExportFormat.PNG:
          return _dxgiTextureService.CreatePngImageStream( dxgiImage );
        case TextureExportFormat.HDR:
          return _dxgiTextureService.CreateHDRImageStream( dxgiImage );
        case TextureExportFormat.BMP:
          return _dxgiTextureService.CreateBmpImageStream( dxgiImage );
        case TextureExportFormat.TIFF:
          return _dxgiTextureService.CreateTiffImageStream( dxgiImage );
        case TextureExportFormat.JPEG:
          return _dxgiTextureService.CreateJpegImageStream( dxgiImage );

        default:
          throw new NotSupportedException( $"Texture Export Format not supported: {Options.ExportFormat}" );
      }
    }

    private async Task WriteFile( Stream exportStream )
    {
      var exportPath = GetExportFilePath();

      // Ensure path exists
      var exportDir = Path.GetDirectoryName( exportPath );
      if ( !Directory.Exists( exportDir ) )
        Directory.CreateDirectory( exportDir );

      using ( var exportFileStream = File.Create( exportPath ) )
      {
        exportStream.Position = 0;
        await exportStream.CopyToAsync( exportFileStream );

        exportFileStream.Flush();
      }
    }

    #endregion

  }

}
