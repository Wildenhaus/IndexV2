using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.Textures
{

  public class DxgiImageStream : Stream
  {

    #region Data Members

    private readonly int _imageIndex;
    private readonly DxgiTextureInfo _textureInfo;
    private readonly Stream _baseStream;

    #endregion

    #region Properties

    public int ImageIndex => _imageIndex;
    public DxgiTextureInfo TextureInfo => _textureInfo;

    public override bool CanRead => _baseStream.CanRead;
    public override bool CanSeek => _baseStream.CanSeek;
    public override bool CanWrite => _baseStream.CanWrite;
    public override long Length => _baseStream.Length;

    public override long Position
    {
      get => _baseStream.Position;
      set => _baseStream.Position = value;
    }

    #endregion

    #region Constructor

    public DxgiImageStream( Stream baseStream, int imageIndex, DxgiTextureInfo textureInfo )
    {
      _baseStream = baseStream;
      _imageIndex = imageIndex;
      _textureInfo = textureInfo;
    }

    #endregion

    #region Overrides

    public override void Flush()
      => _baseStream.Flush();

    public override int Read( byte[] buffer, int offset, int count )
      => _baseStream.Read( buffer, offset, count );

    public override long Seek( long offset, SeekOrigin origin )
      => _baseStream.Seek( offset, origin );

    public override void SetLength( long value )
      => _baseStream.SetLength( value );

    public override void Write( byte[] buffer, int offset, int count )
      => _baseStream.Write( buffer, offset, count );

    protected override void Dispose( bool disposing )
      => _baseStream.Dispose();

    #endregion

  }

}
