using Index.Common;

namespace Index.Jobs
{

  public abstract class CompositeJobBase : JobBase
  {

    #region Data Members

    private readonly List<(int jobKey, IJob job)> _jobs;

    private IJob _currentJob;
    private int _currentJobKey;
    private int _completedJobs;

    #endregion

    #region Constructor

    protected CompositeJobBase()
    {
      _jobs = new List<(int jobKey, IJob job)>();
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      CreateSubJobs();

      Progress.CompletedUnits = 0;
      Progress.TotalUnits = _jobs.Count;
    }

    protected override async Task OnExecuting()
    {
      foreach ( (int jobKey, IJob job) in _jobs )
      {
        if ( IsCancellationRequested )
          return;

        _currentJob = job;
        _currentJobKey = jobKey;

        try
        {
          job.Progress.PropertyChanged += OnSubJobProgressPropertyChanged;
          await job.Execute();
        }
        finally
        {
          job.Progress.PropertyChanged -= OnSubJobProgressPropertyChanged;
        }

        if ( job.State == JobState.Faulted )
        {
          HandleException( job.Exception );
          return;
        }

        await OnSubJobCompleted( jobKey, job );
      }
    }

    #endregion

    #region Private Methods

    protected abstract void CreateSubJobs();

    protected virtual Task OnSubJobCompleted( int jobKey, IJob job ) => Task.CompletedTask;

    protected void AddJob( int jobKey, IJob job )
    {
      _jobs.Add( (jobKey, job) );
    }

    #endregion

    #region Event Handlers

    private void OnSubJobProgressPropertyChanged( object? sender, System.ComponentModel.PropertyChangedEventArgs e )
    {
      var currentJobProgress = _currentJob.Progress;
      switch ( e.PropertyName )
      {
        case nameof( IProgressInfo.PercentCompleted ):
          {
            if ( currentJobProgress.IsIndeterminate )
              return;

            Progress.CompletedUnits = _completedJobs + currentJobProgress.PercentCompleted;
            return;
          }

        case nameof( IProgressInfo.Status ):
          Progress.SubStatus = currentJobProgress.Status;
          return;
      }
    }

    #endregion

  }

}
