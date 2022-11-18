using System;
using System.Windows;
using Index.App.ViewModels;

namespace Index.App.Views
{

  public partial class EditorLoadingView : Window
  {

    public EditorLoadingView( EditorLoadingViewModel viewModel )
    {
      InitializeComponent();
      DataContext = viewModel;
      viewModel.Complete += OnLoadingComplete;
    }

    private void OnLoadingComplete( object? sender, EventArgs e )
    {
      Dispatcher.BeginInvoke( () =>
      {
        DialogResult = true;
        Close();
      } );
    }

  }

}
