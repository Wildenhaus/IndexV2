namespace Index.Domain.Assets
{

  public interface IAssetReference
  {

    string AssetName { get; }
    Type AssetType { get; }

    IFileSystemAssetNode Node { get; }

  }

}
