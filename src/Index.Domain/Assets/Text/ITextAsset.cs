namespace Index.Domain.Assets.Text
{

  public interface ITextAsset : IAsset
  {

    Stream TextStream { get; }

  }

}
