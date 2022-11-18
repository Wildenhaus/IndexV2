using System.Windows;
using System.Windows.Shapes;

namespace Index.UI.Windows
{

  public class IxWindow : Window
  {

    #region Properties

    public bool CanClose { get; set; } = true;

    public Path? TitleBarIcon { get; set; }

    #endregion

    #region Constructor

    static IxWindow()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxWindow ),
        new FrameworkPropertyMetadata( typeof( IxWindow ) ) );
    }

    #endregion

  }

}
