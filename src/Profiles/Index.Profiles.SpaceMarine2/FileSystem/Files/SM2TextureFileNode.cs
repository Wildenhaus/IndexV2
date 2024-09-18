using System.IO.Compression;
using Index.Domain.Assets;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.Assets;

namespace Index.Profiles.SpaceMarine2.FileSystem.Files
{

  public class SM2TextureFileNode : SM2FileSystemNode, IFileSystemAssetNode<DxgiTextureAsset, SM2TextureAssetFactory>
  {

    #region Constructor

    public SM2TextureFileNode( IFileSystemDevice device, ZipArchiveEntry entry, IFileSystemNode parent = null )
      : base( device, entry, parent )
    {
    }

    #endregion

  }

}
