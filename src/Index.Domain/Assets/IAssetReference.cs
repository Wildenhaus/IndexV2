namespace Index.Domain.Assets
{

  public interface IAssetReference : IEquatable<IAssetReference>
  {

    string AssetName { get; }
    Type AssetType { get; }
    Type AssetFactoryType { get; }

    string EditorKey { get; }

    IFileSystemAssetNode Node { get; }

  }

}
