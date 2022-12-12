namespace Index.Domain.Assets.Textures
{

  public interface ITextureAssetImage : IDisposable
  {

    #region Properties

    public int Index { get; }
    Stream PreviewStream { get; }

    #endregion

  }

}
