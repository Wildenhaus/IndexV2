using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.Halo2A.Assets;

namespace Index.Profiles.Halo2A.FileSystem.Files
{

  public class H2ATemplateFileNode : H2AFileSystemNode, IFileSystemAssetNode<H2ATemplateAsset, H2ATemplateAssetFactory>
  {

    #region Properties

    public override string DisplayName
    {
      get => $"{Device.Root.Name}/{Name}";
    }

    #endregion

    #region Constructor

    public H2ATemplateFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, SanitizeName( name ), startOffset, sizeInBytes, parent )
    {
    }

    #endregion

  }

}
