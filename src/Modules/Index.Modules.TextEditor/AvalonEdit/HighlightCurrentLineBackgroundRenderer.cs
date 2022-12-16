using System.Windows;
using System.Windows.Media;
using ICSharpCode.AvalonEdit.Rendering;
using AvalonEditor = ICSharpCode.AvalonEdit.TextEditor;

namespace Index.Modules.TextEditor.AvalonEdit
{

  internal class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
  {

    #region Data Members

    private AvalonEditor _editor;
    private Brush _lineBackgroundBrush;
    private Brush _lineBorderBrush;
    private Pen _borderPen;

    #endregion

    #region Properties

    public KnownLayer Layer => KnownLayer.Background;

    #endregion

    #region Constructor

    public HighlightCurrentLineBackgroundRenderer( AvalonEditor editor )
    {
      _editor = editor;

      _lineBackgroundBrush = ( Brush ) Application.Current.FindResource( "Brushes.Deep.Background.Static" );
      _lineBorderBrush = ( Brush ) Application.Current.FindResource( "Brushes.Deepest.Border.Static" );
      _borderPen = new Pen( _lineBorderBrush, 1 );
    }

    #endregion

    public static void Install( AvalonEditor editor )
    {
      var renderer = new HighlightCurrentLineBackgroundRenderer( editor );
      editor.TextArea.TextView.BackgroundRenderers.Add( renderer );
    }

    #region Public Methods

    public void Draw( TextView textView, DrawingContext drawingContext )
    {
      var editor = _editor;
      if ( editor.Document is null )
        return;

      textView.EnsureVisualLines();
      var currentLine = editor.Document.GetLineByOffset( editor.CaretOffset );
      foreach ( var rect in BackgroundGeometryBuilder.GetRectsForSegment( textView, currentLine ) )
      {
        var borderRect = new Rect( rect.X - 12, rect.Y, textView.ActualWidth + 14, rect.Height );
        drawingContext.DrawRectangle( _lineBackgroundBrush, _borderPen, borderRect );
      }
    }

    #endregion

  }

}
