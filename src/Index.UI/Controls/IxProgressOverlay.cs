using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Index.Common;

namespace Index.UI.Controls
{

  public class IxProgressOverlay : UserControl
  {

    #region Properties

    public static readonly DependencyProperty CancelCommandProperty = DependencyProperty.Register(
      nameof( CancelCommand ),
      typeof( ICommand ),
      typeof( IxProgressOverlay ) );

    public ICommand CancelCommand
    {
      get => ( ICommand ) GetValue( CancelCommandProperty );
      set => SetValue( CancelCommandProperty, value );
    }

    public static readonly DependencyProperty ProgressInfoProperty = DependencyProperty.Register(
      nameof( ProgressInfo ),
      typeof( IProgressInfo ),
      typeof( IxProgressOverlay ) );

    public IProgressInfo ProgressInfo
    {
      get => ( IProgressInfo ) GetValue( ProgressInfoProperty );
      set => SetValue( ProgressInfoProperty, value );
    }

    #endregion

    #region Constructor

    static IxProgressOverlay()
    {
      DefaultStyleKeyProperty.OverrideMetadata(
        typeof( IxProgressOverlay ),
        new FrameworkPropertyMetadata( typeof( IxProgressOverlay ) ) );
    }

    #endregion

  }

}
