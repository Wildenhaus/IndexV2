using System.Windows.Controls;

namespace Index.UI.Views
{

  public class EditorView : ContentControl, IEditorView
  {

    #region Data Members

    private bool _isDisposed;

    #endregion

    #region Properties

    public bool KeepAlive
    {
      get => !_isDisposed;
    }

    #endregion

    #region Constructor

    public EditorView()
    {
      SetResourceReference( TemplateProperty, "EditorViewTemplate" );
    }

    ~EditorView()
    {
      Dispose( false );
    }

    #endregion

    #region IDisposable Methods

    public void Dispose()
      => Dispose( true );

    private void Dispose( bool disposing )
    {
      if ( _isDisposed )
        return;

      OnDisposing( disposing );

      _isDisposed = true;
    }

    protected virtual void OnDisposing( bool dispose )
    {
    }

    #endregion

  }

}
