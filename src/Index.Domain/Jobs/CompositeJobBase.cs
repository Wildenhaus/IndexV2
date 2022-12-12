using Index.Common;
using Prism.Ioc;
using static System.Reflection.Metadata.BlobBuilder;

namespace Index.Jobs
{

  public abstract class CompositeJobBase : JobBase
  {

    #region Data Members

    private readonly IJobManager _jobManager;

    private readonly List<(int jobKey, IJob job)> _jobs;

    private IJob _currentJob;
    private int _currentJobKey;
    private int _completedJobs;

    #endregion

    #region Constructor

    protected CompositeJobBase( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
      _jobManager = container.Resolve<IJobManager>();

      _jobs = new List<(int jobKey, IJob job)>();
    }

    #endregion

    #region Overrides

    protected override async Task OnInitializing()
    {
      CreateSubJobs();

      Progress.CompletedUnits = 0;
      Progress.TotalUnits = _jobs.Count;
      Progress.IsIndeterminate = false;
    }

    protected override async Task OnExecuting()
    {
      SetIndeterminate();
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
          _completedJobs++;
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

    protected int AddJob<TJob>( IParameterCollection parameters = null )
      where TJob : class, IJob
    {
      if ( parameters is null )
        parameters = this.Parameters;

      var job = _jobManager.CreateJob<TJob>( parameters );
      var jobKey = _jobs.Count;

      _jobs.Add( (jobKey, job) );

      return jobKey;
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

            SetCompletedUnits( _completedJobs + currentJobProgress.PercentCompleted );
            return;
          }

        case nameof( IProgressInfo.Status ):
          Progress.SubStatus = currentJobProgress.Status;
          return;
      }
    }

    #endregion

  }

  public abstract class CompositeJobBase<TResult> : CompositeJobBase, IJob<TResult>
  {

    #region Properties

    public TResult? Result { get; private set; }

    #endregion

    #region Constructor

    protected CompositeJobBase( IContainerProvider container, IParameterCollection parameters )
      : base( container, parameters )
    {
    }

    #endregion

    #region Private Methods

    protected void SetResult( TResult result )
      => Result = result;

    #endregion

  }

}
