namespace Index.Domain.Assets.Textures
{

  public class TextureAssetImage : DisposableObject, ITextureAssetImage
  {

    #region Properties

    public int Index { get; }
    public Stream PreviewStream { get; }

    #endregion

    #region Constructor

    public TextureAssetImage( int index, Stream previewStream )
    {
      Index = index;
      PreviewStream = previewStream;
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      PreviewStream?.Dispose();
    }

    #endregion

  }

}
