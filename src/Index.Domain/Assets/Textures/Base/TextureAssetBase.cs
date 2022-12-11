using Index.Domain.Editors;

namespace Index.Domain.Assets.Textures
{

  public abstract class TextureAssetBase : AssetBase, ITextureAsset
  {

    #region Properties

    public override string TypeName => "Texture";
    public override string EditorKey => DefaultEditorKeys.TextureEditor;

    public IReadOnlyList<ITextureAssetImage> Images { get; set; }
    public IReadOnlyDictionary<TextureExportFormat, CreateTextureExportStreamDelegate> ExportDelegates { get; set; }

    #endregion

    #region Constructor

    public TextureAssetBase( IAssetReference assetReference )
      : base( assetReference )
    {
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      foreach ( var image in Images )
        image?.Dispose();
    }

    #endregion

  }

}
