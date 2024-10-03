using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
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

    public void AnimateProgress( double progress )
    {
      ScaleTransform scaleTransform = FillRectangle.RenderTransform as ScaleTransform;

      if ( scaleTransform != null )
      {
        DoubleAnimation progressAnimation = new DoubleAnimation
        {
          To = progress,
          Duration = TimeSpan.FromSeconds( 0.3 ),
          AccelerationRatio = 0.3,
          DecelerationRatio = 0.3
        };

        scaleTransform.BeginAnimation( ScaleTransform.ScaleYProperty, progressAnimation );
      }
    }

  }

}
