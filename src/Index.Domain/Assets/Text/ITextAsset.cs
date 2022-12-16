namespace Index.Domain.Assets.Text
{

  public interface ITextAsset : IAsset, IExportableAsset
  {

    Stream TextStream { get; }

  }

}
