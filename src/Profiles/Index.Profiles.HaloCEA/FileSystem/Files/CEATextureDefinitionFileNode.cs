using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.Assets;

namespace Index.Profiles.HaloCEA.FileSystem.Files
{

  public class CEATextureDefinitionFileNode : CEACacheBlockEntryFileNode, IFileSystemAssetNode<CEATextureDefinitionAsset>
  {

    public Type AssetFactoryType => typeof( CEATextureDefinitionAssetFactory );

    public CEATextureDefinitionFileNode( IFileSystemDevice device, string name, string entryData, IFileSystemNode parent = null )
      : base( device, name, entryData, parent )
    {
    }

  }

}
