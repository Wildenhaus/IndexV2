using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.Assets;
using LibSaber.SpaceMarine2.Structures.Resources;

namespace Index.Profiles.SpaceMarine2.FileSystem.Files
{

  public class SM2SceneResourceFileNode :
    SM2ResourceFileNode<resDESC_SCENE>,
    IFileSystemAssetNode<SM2SceneAsset, SM2SceneAssetFactory>
  {

    public SM2SceneResourceFileNode( 
      IFileSystemDevice device, 
      fioZIP_CACHE_FILE.ENTRY entry, 
      IFileSystemNode parent = null ) 
      : base( device, entry, parent )
    {
    }

  }
}
