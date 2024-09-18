using System.IO.Compression;
using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.SpaceMarine2.Assets;

namespace Index.Profiles.SpaceMarine2.FileSystem.Files
{

  public class SM2TemplateFileNode : SM2FileSystemNode, IFileSystemAssetNode<SM2TemplateAsset, SM2TemplateAssetFactory>
  {
    public SM2TemplateFileNode( IFileSystemDevice device, ZipArchiveEntry entry, IFileSystemNode parent = null ) 
      : base( device, entry, parent )
    {
    }
  }

}
