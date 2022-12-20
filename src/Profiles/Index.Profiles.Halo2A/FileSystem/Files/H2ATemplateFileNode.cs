using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Profiles.Halo2A.Assets;

namespace Index.Profiles.Halo2A.FileSystem.Files
{

  public class H2ATemplateFileNode : H2AFileSystemNode, IFileSystemAssetNode<H2ATemplateAsset, H2ATemplateAssetFactory>
  {

    #region Properties

    public override string DisplayName
    {
      get => $"{Device.Root.Name}/{Name}";
    }

    #endregion

    #region Constructor

    public H2ATemplateFileNode( IFileSystemDevice device, string name, long startOffset, long sizeInBytes, IFileSystemNode parent = null )
      : base( device, name, startOffset, sizeInBytes, parent )
    {
    }

    #endregion

  }

}
