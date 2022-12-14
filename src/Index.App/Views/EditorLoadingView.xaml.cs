using System;
using System.Threading.Tasks;
using System.Windows;
using Index.App.ViewModels;
using Index.UI.Windows;

namespace Index.App.Views
{

  public partial class EditorLoadingView : Window
  {

    public EditorLoadingView( EditorLoadingViewModel viewModel )
    {
      DataContext = viewModel;
      InitializeComponent();
      viewModel.Complete += OnLoadingComplete;

      // TODO: Potential Race Condition?
      Task.Factory.StartNew( viewModel.Initialize, TaskCreationOptions.LongRunning );
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
