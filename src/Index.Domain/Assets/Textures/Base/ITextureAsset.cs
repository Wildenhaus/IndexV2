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

    TextureType TextureType { get; }
    IEnumerable<(string, string)> TextureInformation { get; }

    IReadOnlyList<ITextureAssetImage> Images { get; }

    #endregion

  }

}
