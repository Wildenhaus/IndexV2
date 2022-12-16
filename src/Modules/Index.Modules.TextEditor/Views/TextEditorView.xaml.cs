using ICSharpCode.AvalonEdit.Search;
using Index.Modules.TextEditor.AvalonEdit;
using Index.UI.Views;

namespace Index.Modules.TextEditor.Views
{

  public partial class TextEditorView : EditorView
  {

    #region Constructor

    public TextEditorView()
    {
      InitializeComponent();
      SearchPanel.Install( TextEditor.TextArea );
      HighlightCurrentLineBackgroundRenderer.Install( TextEditor );
    }

    #endregion



  }

}
