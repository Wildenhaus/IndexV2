using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.Domain.Assets.Textures
{

  public delegate Stream CreateTextureExportStreamDelegate( IAsset asset, TextureExportFormat exportFormat );

}
