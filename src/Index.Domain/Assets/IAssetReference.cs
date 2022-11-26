namespace Index.Domain.Assets
{

  public interface IAssetReference
  {

    string AssetName { get; }
    Type AssetType { get; }
    Type AssetFactoryType { get; }

    string EditorKey { get; }

    IFileSystemAssetNode Node { get; }

  }

}
