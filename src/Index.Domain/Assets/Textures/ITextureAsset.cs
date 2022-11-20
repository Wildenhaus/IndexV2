using Index.Domain.Editors;

namespace Index.Domain.Assets.Textures
{

  public interface ITextureAsset : IAsset
  {

    #region Properties

    string IAsset.TypeName => "Texture";
    string IAsset.EditorKey => DefaultEditorKeys.TextureEditor;

    int Width { get; }
    int Height { get; }
    int Depth { get; }

    #endregion

  }

}
