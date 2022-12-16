using Index.Domain.Editors;

namespace Index.Domain.Assets.Textures
{

  public abstract class TextureAssetBase : AssetBase, ITextureAsset
  {

    #region Properties

    public abstract Type ExportOptionsType { get; }
    public abstract Type ExportJobType { get; }

    public override string TypeName => "Texture";
    public override string EditorKey => DefaultEditorKeys.TextureEditor;

    public abstract int Width { get; }
    public abstract int Height { get; }

    public TextureType TextureType { get; }
    public IReadOnlyList<ITextureAssetImage> Images { get; }
    public IEnumerable<(string, string)> TextureInformation => GetTextureInformation();

    public Dictionary<string, Stream> AdditionalData { get; }

    #endregion

    #region Constructor

    public TextureAssetBase(
      IAssetReference assetReference,
      TextureType textureType,
      IReadOnlyList<ITextureAssetImage> images )
      : base( assetReference )
    {
      TextureType = textureType;
      Images = images;
      AdditionalData = new Dictionary<string, Stream>();
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
      yield return ("Type", TextureType.ToString());
      yield return ("Images", Images.Count.ToString());
      yield return ("Dimensions", $"{Width}x{Height}");
    }

    #endregion

  }

}
