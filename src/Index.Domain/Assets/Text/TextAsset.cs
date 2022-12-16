using Index.Domain.Editors;

namespace Index.Domain.Assets.Text
{

  public class TextAsset : AssetBase, ITextAsset
  {

    #region Properties

    public override string TypeName => "Text";
    public override string EditorKey => DefaultEditorKeys.TextEditorKey;

    public Stream TextStream { get; set; }

    #endregion

    #region Constructor

    public TextAsset( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      base.OnDisposing();
      TextStream?.Dispose();
    }

    #endregion

  }

}
