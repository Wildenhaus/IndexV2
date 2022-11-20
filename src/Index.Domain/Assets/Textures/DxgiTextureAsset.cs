using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.Domain.Assets.Textures
{

  public class DxgiTextureAsset : TextureAsset, IDxgiTextureAsset
  {

    #region Properties

    public int MipMapCount { get; set; }
    public int FaceCount { get; set; }
    public DxgiTextureFormat Format { get; set; }

    #endregion

  }

}
