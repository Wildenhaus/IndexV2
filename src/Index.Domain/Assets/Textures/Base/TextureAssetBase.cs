using Index.Domain.Editors;

namespace Index.Domain.Assets.Textures
{

  public abstract class TextureAssetBase : AssetBase, ITextureAsset
  {

    #region Properties

    public override string TypeName => "Texture";
    public override string EditorKey => DefaultEditorKeys.TextureEditor;

    public int Width { get; set; }
    public int Height { get; set; }

    public TextureType TextureType { get; set; }
    public IEnumerable<(string, string)> TextureInformation => GetTextureInformation();

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

    #region Private Methods

    protected virtual IEnumerable<(string, string)> GetTextureInformation()
    {
      yield return ("Images", Images.Count.ToString());
      yield return ("Dimensions", $"{Width}x{Height}");
    }

    #endregion

  }

}
