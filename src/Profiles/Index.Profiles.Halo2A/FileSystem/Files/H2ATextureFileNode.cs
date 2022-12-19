using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Domain.FileSystem;
using Index.Profiles.Halo2A.Assets;

namespace Index.Profiles.Halo2A.FileSystem.Files
{

  public class H2ATextureFileNode : H2AFileSystemNode, IFileSystemAssetNode<DxgiTextureAsset, H2ATextureAssetFactory>
  {

    #region Constructor

    public H2ATextureFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, name, startOffset, sizeInBytes, parent )
    {
    }

    #endregion

  }

}
