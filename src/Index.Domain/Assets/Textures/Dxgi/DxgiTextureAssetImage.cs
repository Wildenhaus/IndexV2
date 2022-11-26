using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Textures;

namespace Index.Domain.Assets.Textures.Dxgi
{

  public class DxgiTextureAssetImage : IDxgiTextureAssetImage
  {

    #region Data Members

    private readonly DxgiTextureInfo _textureInfo;

    #endregion

    #region Properties

    public int Index { get; }

    public int Width => _textureInfo.Width;
    public int Height => _textureInfo.Height;
    public int Depth => _textureInfo.Depth;

    public int MipMapCount => _textureInfo.MipCount;
    public int FaceCount => _textureInfo.FaceCount;
    public DxgiTextureFormat Format => _textureInfo.Format;

    public Stream PreviewStream { get; set; }
    public IReadOnlyDictionary<TextureExportFormat, CreateTextureExportStreamDelegate> ExportDelegates { get; }

    #endregion

    #region Constructor

    public DxgiTextureAssetImage( int imageIndex, DxgiTextureInfo textureInfo )
    {
      Index = imageIndex;
      _textureInfo = textureInfo;
    }

    #endregion

  }

}
