using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    protected override Task ExportAsset()
    {
      EnsureExportDirectoryExists();

      if ( RequiresPostProcessing() )
        return ExportAssetWithPostProcessing();
      else
        return ExportAssetFromDxgi();
    }

    #endregion

    #region DXGI Route

    private Task ExportAssetFromDxgi()
    {
      if ( Options.ExportFormat == ImageFormat.DDS )
        return WriteDDSFile();
      else
        return WriteDxgiFiles();

    }

    private async Task WriteDDSFile()
    {
      SetStatus( "Writing File" );
      SetIndeterminate();

      using ( var ddsStream = _dxgiTextureService.CreateDDSStream( Asset.DxgiImage ) )
        await WriteFile( GetExportFilePath(), ddsStream );
    }

    private async Task WriteDxgiFiles()
    {
      var streams = GetDxgiExportStreams();
      var imageCount = streams.Length;

      if ( imageCount > 1 )
      {
        SetStatus( "Writing Files" );
        SetTotalUnits( imageCount );
        SetCompletedUnits( 0 );
      }
      else
      {
        SetStatus( "Writing File" );
        SetIndeterminate();
      }

      for ( var i = 0; i < imageCount; i++ )
      {
        var exportPath = GetExportFilePath( i );
        var exportStream = streams[ i ];
        await WriteFile( exportPath, exportStream );

        if ( imageCount > 1 )
          IncreaseCompletedUnits( 1 );
      }
    }

    private Stream[] GetDxgiExportStreams()
    {
      var dxgiImage = Asset.DxgiImage;
      switch ( Options.ExportFormat )
      {
        case ImageFormat.TGA:
          return _dxgiTextureService.CreateTgaImageStreams( dxgiImage );
        case ImageFormat.PNG:
          return _dxgiTextureService.CreatePngImageStreams( dxgiImage );
        case ImageFormat.HDR:
          return _dxgiTextureService.CreateHDRImageStreams( dxgiImage );
        case ImageFormat.BMP:
          return _dxgiTextureService.CreateBmpImageStreams( dxgiImage );
        case ImageFormat.TIFF:
          return _dxgiTextureService.CreateTiffImageStreams( dxgiImage );
        case ImageFormat.JPEG:
          return _dxgiTextureService.CreateJpegImageStreams( dxgiImage );

        default:
          throw new NotSupportedException( $"Texture Export Format not supported: {Options.ExportFormat}" );
      }
    }

    #endregion

    #region Post-Processing Route

    private async Task ExportAssetWithPostProcessing()
    {
      var processor = await ApplyPostProcessing();
      await WritePostProcessedFiles( processor );

      processor?.Dispose();
    }

    private async Task<ImagePostProcessor> ApplyPostProcessing()
    {
      SetStatus( "Post-Processing Texture" );
      SetIndeterminate();

      var processor = _dxgiTextureService.CreatePostProcessor( Asset.DxgiImage );
      if ( Asset.TextureType == TextureType.Normals )
        ApplyNormalMapPostProcessing( processor );

      return processor;
    }

    private void ApplyNormalMapPostProcessing( ImagePostProcessor processor )
    {
      if ( Options.NormalMapFormat == NormalMapFormat.OpenGL )
      {
        SetSubStatus( "Converting Normal Map to OpenGL Format" );
        processor.InvertGreenChannel();
      }
      if ( Options.RecalculateZChannel )
      {
        SetSubStatus( "Recalculating Z Channel" );
        processor.RecalculateZChannel();
      }
    }

    private async Task WritePostProcessedFiles( ImagePostProcessor processor )
    {
      SetIndeterminate();

      var imageCount = processor.ImageCount;
      if ( imageCount > 1 )
      {
        SetStatus( "Writing Files" );
        SetTotalUnits( imageCount );
        SetCompletedUnits( 0 );
      }
      else
      {
        SetStatus( "Writing File" );
        SetIndeterminate();
      }

      for ( var i = 0; i < imageCount; i++ )
      {
        var exportPath = GetExportFilePath( i );
        var exportStream = await processor.SaveAsync( i, Options.ExportFormat );
        await WriteFile( exportPath, exportStream );

        if ( imageCount > 1 )
          IncreaseCompletedUnits( 1 );
      }
    }

    #endregion

    #region Private Methods

    private bool RequiresPostProcessing()
    {
      if ( Options.ExportFormat == ImageFormat.DDS )
        return false;

      if ( Asset.TextureType == TextureType.Normals )
      {
        if ( Options.RecalculateZChannel )
          return true;

        if ( Options.NormalMapFormat == NormalMapFormat.OpenGL )
          return true;
      }

      return false;
    }

    private string GetExportFilePath( int imageIndex = 0 )
    {
      string assetName;
      if ( imageIndex > 0 )
        assetName = $"{Asset.AssetName}_{imageIndex}";
      else
        assetName = Asset.AssetName;

      var fileExt = Options.ExportFormat.GetFileExtension();
      assetName = Path.ChangeExtension( assetName, fileExt );

      var exportPath = Path.Combine( Options.ExportPath, assetName );
      return exportPath;
    }

    private void EnsureExportDirectoryExists()
    {
      var exportPath = GetExportFilePath();
      var exportDir = Path.GetDirectoryName( exportPath );

      if ( !Directory.Exists( exportDir ) )
        Directory.CreateDirectory( exportDir );
    }

    private async Task WriteFile( string exportPath, Stream exportStream )
    {
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
