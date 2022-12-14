using Assimp;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.Textures;
using Prism.Ioc;

namespace Index.Domain.Assets.Meshes
{

  public class MeshAssetExportJob : AssetExportJobBase<MeshAsset, MeshAssetExportOptions>
  {

    #region Properties

    protected IJobManager JobManager { get; }

    #endregion

    #region Constructor

    public MeshAssetExportJob( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      JobManager = container.Resolve<IJobManager>();
    }

    #endregion

    #region Overrides

    protected override async Task ExportAsset()
    {
      if ( Options.ExportTextures )
        await ExportTextures();

      if ( Options.RemoveLODs || Options.RemoveVolumes )
        RemoveLodsAndVolumes();

      WriteFile();
    }

    #endregion

    #region Private Methods

    private string GetExportFilePath()
    {
      var exportPath = Options.ExportPath;
      var fileExt = Options.ExportFormat.GetFileExtension();

      var assetName = Path.ChangeExtension( Asset.AssetName, "" );
      var rootAssetName = Path.GetFileNameWithoutExtension( Asset.AssetName );
      var exportFileName = Path.ChangeExtension( rootAssetName, fileExt );

      exportPath = Path.Combine( exportPath, assetName, exportFileName );
      return exportPath;
    }

    private void WriteFile()
    {
      SetStatus( "Writing File" );
      SetIndeterminate();

      var exportPath = GetExportFilePath();

      // Ensure path exists
      var exportDir = Path.GetDirectoryName( exportPath );
      if ( !Directory.Exists( exportDir ) )
        Directory.CreateDirectory( exportDir );

      var assimpScene = Asset.AssimpScene;
      var assimpFormatId = Options.ExportFormat.GetAssimpFormatId();
      using ( var ctx = new AssimpContext() )
      {
        ctx.ExportFile( assimpScene, exportPath, assimpFormatId );
      }
    }

    private async Task ExportTextures()
    {
      if ( Asset.Textures.Count == 0 )
        return;

      SetStatus( "Exporting Textures" );
      SetTotalUnits( Asset.Textures.Count );
      SetCompletedUnits( 0 );


      // Place the textures in the same directory as the model
      var textureDir = Path.GetDirectoryName( GetExportFilePath() );
      Options.TextureOptions.ExportPath = textureDir;

      // Update Materials
      var updateMaterialTask = UpdateMaterialsWithExportedTexturePath();

      var textures = Asset.Textures.Values;
      foreach ( var texture in textures )
      {
        if ( !( texture is IExportableAsset exportableTexture ) )
        {
          StatusList.AddWarning( "Texture", "Texture {0} is not marked as exportable and will be skipped.", texture.AssetName );
          continue;
        }

        var parameters = new ParameterCollection();
        parameters.Set( "Asset", texture );
        parameters.Set( texture.AssetReference );
        parameters.Set( "Options", Options.TextureOptions );

        var textureExportJobType = exportableTexture.ExportJobType;
        var exportTextureJob = JobManager.CreateJob( textureExportJobType, parameters );
        JobManager.StartJob( exportTextureJob );


        await exportTextureJob.Completion;

        IncreaseCompletedUnits( 1 );
      }

      await UpdateMaterialsWithExportedTexturePath();
    }

    private Task UpdateMaterialsWithExportedTexturePath()
    {
      return Task.Run( () =>
      {
        var textureFileExtension = Options.TextureOptions.ExportFormat.GetFileExtension();

        foreach ( var material in Asset.AssimpScene.Materials )
        {
          var textureSlots = material.GetAllMaterialTextures();
          foreach ( var textureSlot in textureSlots )
            material.RemoveMaterialTexture( textureSlot );
          foreach ( var textureSlot in textureSlots )
            material.AddMaterialTexture( FixupTextureSlotFileExtension( textureSlot, textureFileExtension ) );
        }
      } );
    }

    private TextureSlot FixupTextureSlotFileExtension( TextureSlot textureSlot, string textureFileExtension )
    {
      if ( string.IsNullOrEmpty( textureSlot.FilePath ) )
        return textureSlot;

      var newPath = Path.ChangeExtension( textureSlot.FilePath, textureFileExtension );
      return new TextureSlot(
        newPath,
        textureSlot.TextureType,
        textureSlot.TextureIndex,
        textureSlot.Mapping,
        textureSlot.UVIndex,
        textureSlot.BlendFactor,
        textureSlot.Operation,
        textureSlot.WrapModeU,
        textureSlot.WrapModeV,
        textureSlot.Flags
        );
    }

    private void RemoveLodsAndVolumes()
    {
      SetStatus( "Filtering Meshes" );
      SetIndeterminate();

      var removeMeshNames = new HashSet<string>();

      if ( Options.RemoveLODs )
        foreach ( var lodMeshName in Asset.LodMeshNames )
          removeMeshNames.Add( lodMeshName );

      if ( Options.RemoveVolumes )
        foreach ( var volumeMeshName in Asset.VolumeMeshNames )
          removeMeshNames.Add( volumeMeshName );

      if ( removeMeshNames.Count == 0 )
        return;

      var meshes = Asset.AssimpScene.Meshes;
      var indicesToRemove = new HashSet<int>();
      for ( var i = 0; i < meshes.Count; i++ )
      {
        var mesh = meshes[ i ];
        if ( removeMeshNames.Contains( mesh.Name ) )
          indicesToRemove.Add( i );
      }

      RemoveLodsAndVolumes( Asset.AssimpScene.RootNode, indicesToRemove );
    }

    private void RemoveLodsAndVolumes( Node node, HashSet<int> indicesToRemove )
    {
      var meshIndices = node.MeshIndices.ToArray();
      foreach ( var index in meshIndices )
        if ( indicesToRemove.Contains( index ) )
          node.MeshIndices.Remove( index );

      foreach ( var child in node.Children )
        RemoveLodsAndVolumes( child, indicesToRemove );
    }

    #endregion

  }

}
