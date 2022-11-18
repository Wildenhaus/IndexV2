using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Controls
{

  // https://github.com/blackspike/Xaml-Spinners-WPF
  public partial class IxSpinner : UserControl
  {

    public static DependencyProperty SizeProperty = DependencyProperty.Register(
      nameof( Size ),
      typeof( double ),
      typeof( IxSpinner ) );

    public double Size
    {
      get => ( double ) GetValue( SizeProperty );
      set => SetValue( SizeProperty, value );
    }

    public IxSpinner()
    {
      InitializeComponent();
    }

  }

}
