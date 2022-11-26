namespace Index.Domain.Assets
{

  public interface IAssetReference
  {

    string AssetName { get; }
    Type AssetType { get; }
    Type AssetFactoryType { get; }

    IFileSystemAssetNode Node { get; }

  }

}
