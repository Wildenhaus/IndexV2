using Index.UI.Views;

namespace Index.Modules.MeshEditor.Views
{

  public partial class MeshEditorView : EditorView
  {

    #region Constructor

    public MeshEditorView()
    {
      InitializeComponent();
    }

    #endregion

    #region Overrides

    protected override void OnDisposing( bool dispose )
    {
      base.OnDisposing( dispose );
      MeshViewer?.Dispose();
    }

    #endregion

  }

}
