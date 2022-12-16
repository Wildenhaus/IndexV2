using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.HaloCEA.Assets;

namespace Index.Profiles.HaloCEA.FileSystem.Files
{

  public class CEAScriptFileNode : CEACacheBlockEntryFileNode, IFileSystemAssetNode<CEAScriptAsset>
  {

    public Type AssetFactoryType => typeof( CEAScriptAssetFactory );

    public CEAScriptFileNode( IFileSystemDevice device, string name, string entryData, IFileSystemNode parent = null )
      : base( device, name, entryData, parent )
    {
    }

  }

}
