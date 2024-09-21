using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.Assets;
using LibSaber.SpaceMarine2.Structures.Resources;

namespace Index.Profiles.SpaceMarine2.FileSystem.Files
{
  public class SM2TextureDefinitionResourceFileNode :
    SM2ResourceFileNode<resDESC_TD>,
    IFileSystemAssetNode<SM2TextureDefinitionAsset, SM2TextAssetFactory<SM2TextureDefinitionAsset>>
  {

    public SM2TextureDefinitionResourceFileNode( 
      IFileSystemDevice device, 
      fioZIP_CACHE_FILE.ENTRY entry, 
      IFileSystemNode parent = null ) 
      : base( device, entry, parent )
    {
    }

  }
}
