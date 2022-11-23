using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Controls
{

  public class IxButton : Button
  {

    #region Properties

    public static DependencyProperty IconProperty = DependencyProperty.Register(
     nameof( Icon ),
     typeof( SegoeIcon ),
     typeof( IxButton ) );

    public SegoeIcon Icon
    {
      get => ( SegoeIcon ) GetValue( IconProperty );
      set => SetValue( IconProperty, value );
    }

    #endregion

    #region Constructor

    static IxButton()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxButton ),
        new FrameworkPropertyMetadata( typeof( IxButton ) ) );
    }

    #endregion

  }

}
