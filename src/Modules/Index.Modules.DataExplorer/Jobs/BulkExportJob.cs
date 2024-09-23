using System;
using System.Collections.Generic;
using System.Linq;
using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Domain.Assets.Text;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Jobs;
using Prism.Ioc;
using Serilog;

namespace Index.Modules.DataExplorer.Jobs
{

  public class BulkExportJob : CompositeJobBase
  {

    #region Data Members

    private List<IAssetReference> _assetsToExport;
    private IReadOnlyDictionary<Type, AssetExportOptions> _exportOptions;

    #endregion

    #region Constructor

    public BulkExportJob(
      IContainerProvider containerProvider, 
      IParameterCollection parameterCollection )
      : base( containerProvider, parameterCollection )
    {
      Name = "Bulk Asset Export";
      ContinueOnSubJobFaulted = true;

      _assetsToExport = parameterCollection.Get<IEnumerable<IAssetReference>>( "assetsToExport" ).ToList();
      _exportOptions = parameterCollection.Get<IReadOnlyDictionary<Type, AssetExportOptions>>( "exportOptions" );
    }

    #endregion

    #region Overrides

    protected override void CreateSubJobs()
    {
      foreach ( var asset in _assetsToExport )
      {
        var options = GetOptions( asset );

        var parameters = new ParameterCollection();
        parameters.Set( "AssetReference", asset );
        parameters.Set( "Options", options );

        switch(options)
        {
          case MeshAssetExportOptions meshOptions:
            AddJob<MeshAssetExportJob>( parameters );
            break;
          case TextAssetExportOptions textOptions:
            AddJob<TextAssetExportJob>( parameters );
            break;
          case DxgiTextureExportOptions dxgiOptions:
            AddJob<DxgiTextureAssetExportJob>( parameters );
            break;

          default:
            Log.Error( "Unimplemented bulk export asset type: {typeName}", options.GetType().FullName );
            break;
        }
      }
    }

    #endregion

    #region Private Methods

    private AssetExportOptions GetOptions( IAssetReference asset )
      => _exportOptions[ asset.AssetType ];

    #endregion

  }

}
