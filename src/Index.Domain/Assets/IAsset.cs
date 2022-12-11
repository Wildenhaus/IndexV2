namespace Index.Domain.Assets
{

  public interface IAsset : IDisposable
  {

    string TypeName { get; }
    string EditorKey { get; }

    string AssetName { get; }
    IAssetReference AssetReference { get; }
    IAssetLoadContext AssetLoadContext { get; internal set; }

  }

}
