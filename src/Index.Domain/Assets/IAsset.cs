namespace Index.Domain.Assets
{

  public interface IAsset
  {

    string TypeName { get; }
    string EditorKey { get; }

    string AssetName { get; }

  }

}
