using System.Windows.Controls;

namespace Index.UI.Views
{

  public class EditorView : ContentControl
  {

    #region Constructor

    public EditorView()
    {
      SetResourceReference( TemplateProperty, "EditorViewTemplate" );
    }

    #endregion

  }

}
