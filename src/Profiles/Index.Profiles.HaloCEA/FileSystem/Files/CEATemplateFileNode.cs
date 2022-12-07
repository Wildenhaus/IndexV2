using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.Assets;

namespace Index.Profiles.HaloCEA.FileSystem.Files
{

  public class CEATemplateFileNode : CEAFileNode, IFileSystemAssetNode<MeshAsset, CEATemplateAssetFactory>
  {


    public override string DisplayName
    {
      get => $"{Device.Root.Name}/{Name}";
    }

    #region Constructor

    public CEATemplateFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    #endregion

  }

}
