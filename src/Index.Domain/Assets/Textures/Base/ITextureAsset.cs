using Index.Domain.Editors;

namespace Index.Domain.Assets.Textures
{

  public interface ITextureAsset : IAsset
  {

    #region Properties

    string IAsset.TypeName => "Texture";
    string IAsset.EditorKey => DefaultEditorKeys.TextureEditor;

    IReadOnlyList<ITextureAssetImage> Images { get; }
    IReadOnlyDictionary<TextureExportFormat, CreateTextureExportStreamDelegate> ExportDelegates { get; set; }

    #endregion

  }

}
