using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.Assets;

namespace Index.Profiles.HaloCEA.FileSystem.Files
{

  public class CEASceneFileNode : CEAFileNode, IFileSystemAssetNode<CEASceneAsset, CEASceneAssetFactory>
  {

    #region Properties

    public override string DisplayName
    {
      get => $"{Device.Root.Name}/{Name}";
    }

    #endregion

    #region Constructor

    public CEASceneFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

    #endregion

  }

}
