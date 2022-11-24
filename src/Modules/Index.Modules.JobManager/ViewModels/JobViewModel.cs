using System;
using Index.Common;
using Index.Jobs;
using Index.UI.ViewModels;

namespace Index.Modules.JobManager.ViewModels
{

  public class JobViewModel : ViewModelBase
  {

    private IJob _job;

    public string Name { get; private set; }
    public JobState State { get; private set; }
    public string Status { get; private set; }
    public double PercentCompleted { get; private set; }

    public JobViewModel( IJob job )
    {
      _job = job;

      SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
      _job.PropertyChanged += OnJobPropertyChanged;
      _job.Progress.PropertyChanged += OnJobProgressChanged;

      _job.Cancelled += OnJobExecutionCompleted;
      _job.Faulted += OnJobExecutionCompleted;
      _job.Completed += OnJobExecutionCompleted;

      Name = _job.Name;
      State = _job.State;
      Status = _job.Progress.Status;
      PercentCompleted = _job.Progress.PercentCompleted;
    }

    private void UnsubscribeFromEvents()
    {
      _job.PropertyChanged -= OnJobPropertyChanged;
      _job.Progress.PropertyChanged -= OnJobProgressChanged;

      _job.Cancelled -= OnJobExecutionCompleted;
      _job.Faulted -= OnJobExecutionCompleted;
      _job.Completed -= OnJobExecutionCompleted;
    }

    private void OnJobPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
      if ( e.PropertyName == nameof( IJob.State ) )
        State = _job.State;
    }

    private void OnJobProgressChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
      switch ( e.PropertyName )
      {
        case nameof( IProgressInfo.Status ):
          Status = _job.Progress.Status;
          break;

        case nameof( IProgressInfo.PercentCompleted ):
          PercentCompleted = _job.Progress.PercentCompleted;
          break;
      }
    }

    private void OnJobExecutionCompleted( object? sender, EventArgs e )
    {
      UnsubscribeFromEvents();
      _job = null;
    }

  }

}
