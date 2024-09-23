using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.Assets;
using LibSaber.SpaceMarine2.Structures.Resources;

namespace Index.Profiles.SpaceMarine2.FileSystem.Files
{

  public class SM2TemplateResourceFileNode : 
    SM2ResourceFileNode<resDESC_TPL>, 
    IFileSystemAssetNode<SM2TemplateAsset, SM2TemplateAssetFactory>
  {

    #region Constructor

    public SM2TemplateResourceFileNode( 
      IFileSystemDevice device, 
      fioZIP_CACHE_FILE.ENTRY entry, 
      IFileSystemNode parent = null )
      : base( device, entry, parent )
    {
    }

    #endregion

    #region Overrides

    public override bool IsHidden
    {
      get
      {
        if ( ResourceDescription is null ) return true;
        if ( string.IsNullOrEmpty( ResourceDescription.tpl ) ) return true;
        //if ( string.IsNullOrEmpty( ResourceDescription.tplData ) ) return true;
        if ( DisplayName.StartsWith( "__x" ) ) return true;

        return false;
      }
    }

    #endregion

  }

}
