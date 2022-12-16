using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.Assets;

namespace Index.Profiles.HaloCEA.FileSystem.Files
{

  public class CEAShaderCodeFileNode : CEAFileNode, IFileSystemAssetNode<CEAShaderCodeAsset>
  {

    public Type AssetFactoryType => typeof( CEAShaderCodeAssetFactory );

    public CEAShaderCodeFileNode( IFileSystemDevice device, string name, IFileSystemNode parent = null )
      : base( device, name, parent )
    {
    }

  }

}
