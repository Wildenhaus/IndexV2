using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Index.Modules.MeshEditor.Controls
{

  public partial class MeshTypeIndicator : UserControl
  {

    #region Properties

    public static readonly DependencyProperty ColorProperty = DependencyProperty.Register(
      nameof( Color ),
      typeof( Brush ),
      typeof( MeshTypeIndicator ) );

    public Brush Color
    {
      get => ( Brush ) GetValue( ColorProperty );
      set => SetValue( ColorProperty, value );
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
      nameof( Text ),
      typeof( string ),
      typeof( MeshTypeIndicator ) );

    public string Text
    {
      get => ( string ) GetValue( TextProperty );
      set => SetValue( TextProperty, value );
    }

    #endregion

    #region Constructor

    public MeshTypeIndicator()
    {
      InitializeComponent();
    }

    #endregion

  }

}
