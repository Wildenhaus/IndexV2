using System;
using System.Threading.Tasks;
using System.Windows;
using Index.App.ViewModels;

namespace Index.App.Views
{

  public partial class EditorLoadingView : Window
  {

    public Exception Exception { get; private set; }

    public EditorLoadingView( EditorLoadingViewModel viewModel )
    {
      DataContext = viewModel;
      InitializeComponent();
      viewModel.Complete += OnLoadingComplete;
      viewModel.Faulted += OnLoadingException;

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

    private void OnLoadingException( object? sender, Exception e )
    {
      Dispatcher.BeginInvoke( () =>
      {
        DialogResult = false;
        Exception = e;
        Close();
      } );
    }

  }

}
