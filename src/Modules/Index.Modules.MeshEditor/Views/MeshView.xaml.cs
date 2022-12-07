using System.Windows;
using System.Windows.Controls;
using HelixToolkit.Wpf.SharpDX;
using Index.Modules.MeshEditor.ViewModels;

namespace Index.Modules.MeshEditor.Views
{

  public partial class MeshView : UserControl
  {

    public MeshView()
    {
      InitializeComponent();
      DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged( object sender, DependencyPropertyChangedEventArgs e )
    {
      var context = e.NewValue as MeshEditorViewModel;
      context.Viewport = ViewportControl;
    }

  }

}
