using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.Halo2A.Assets;

namespace Index.Profiles.Halo2A.FileSystem.Files
{

  public class H2AShaderCodeFileNode : H2AFileSystemNode, IFileSystemAssetNode<H2AShaderCodeAsset, H2ATextAssetFactory<H2AShaderCodeAsset>>
  {
    public H2AShaderCodeFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, name, startOffset, sizeInBytes, parent )
    {
    }
  }

}
