using System.Runtime.Serialization;

namespace Index.Domain.Assets
{

  public class AssetReference : IAssetReference
  {

    #region Properties

    public string AssetName { get; }
    public Type AssetType { get; }
    public Type AssetFactoryType { get; }

    public string EditorKey => GetEditorKey();

    public IFileSystemAssetNode Node { get; }

    #endregion

    #region Constructor

    public AssetReference( IFileSystemAssetNode node, string assetName = null )
    {
      Node = node;
      AssetType = node.AssetType;
      AssetFactoryType = node.AssetFactoryType;
      AssetName = assetName ?? node.DisplayName;
    }

    public AssetReference( Type assetType, IFileSystemAssetNode node, string assetName = null )
    {
      Node = node;
      AssetType = assetType;
      AssetFactoryType = node.AssetFactoryType;
      AssetName = assetName ?? node.DisplayName;
    }

    #endregion

    #region Private Methods

    private string GetEditorKey()
    {
      var dummy = ( IAsset ) FormatterServices.GetUninitializedObject( AssetType );
      return dummy.EditorKey;
    }

    #endregion

    #region Overrides

    public bool Equals( IAssetReference other )
    {
      return this.AssetType == other.AssetType
          && this.AssetName == other.AssetName;
    }

    public override bool Equals( object? obj )
      => obj is IAssetReference other && Equals( other );

    public override int GetHashCode()
      => AssetName.GetHashCode();

    #endregion

  }

}
