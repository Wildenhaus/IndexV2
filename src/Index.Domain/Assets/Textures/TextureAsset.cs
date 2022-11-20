using Index.Domain.Editors;

namespace Index.Domain.Assets.Textures
{

  public class TextureAsset : AssetBase, ITextureAsset
  {

    #region Properties

    public override string TypeName => "Texture";
    public override string EditorKey => DefaultEditorKeys.TextureEditor;

    public string AssetName { get; set; }

    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }

    #endregion

  }

}
