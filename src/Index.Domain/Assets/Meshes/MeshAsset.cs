using Assimp;
using Index.Domain.Assets.Textures;
using Index.Domain.Editors;

namespace Index.Domain.Assets.Meshes
{

  [AssetExportOptionsType(typeof(MeshAssetExportOptions))]
  public class MeshAsset : AssetBase, IMeshAsset
  {

    #region Properties

    public override string TypeName => "Mesh";
    public override string EditorKey => DefaultEditorKeys.MeshEditorKey;

    Type IExportableAsset.ExportJobType => typeof( MeshAssetExportJob );
    Type IExportableAsset.ExportOptionsType => typeof( MeshAssetExportOptions );

    public Scene AssimpScene { get; set; }
    public IReadOnlyDictionary<string, ITextureAsset> Textures { get; set; }

    public ISet<string> LodMeshNames { get; set; }
    public ISet<string> VolumeMeshNames { get; set; }

    #endregion

    #region Constructor

    public MeshAsset( IAssetReference assetReference )
      : base( assetReference )
    {
      LodMeshNames = new HashSet<string>();
      VolumeMeshNames = new HashSet<string>();
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      AssimpScene = null;
      Textures = null;
      LodMeshNames = null;
      VolumeMeshNames = null;
    }

    #endregion

  }

}
