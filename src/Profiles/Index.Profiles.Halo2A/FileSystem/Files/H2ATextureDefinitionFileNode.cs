using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.Halo2A.Assets;

namespace Index.Profiles.Halo2A.FileSystem.Files
{

  public class H2ATextureDefinitionFileNode : H2AFileSystemNode, IFileSystemAssetNode<H2ATextureDefinitionAsset, H2ATextAssetFactory<H2ATextureDefinitionAsset>>
  {
    public H2ATextureDefinitionFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, name, startOffset, sizeInBytes, parent )
    {
    }
  }

}
