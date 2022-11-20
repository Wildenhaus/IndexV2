using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.Jobs
{

  public abstract class CompositeJobBase : JobBase
  {

    #region Data Members

    private readonly List<(int jobKey, IJob job)> _jobs;

    private IJob _currentJob;
    private int _currentJobKey;

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
      await CreateSubJobs();

      CompletedUnits = 0;
      TotalUnits = _jobs.Count;
    }

    protected override async Task OnExecuting()
    {
      foreach ( (int jobKey, IJob job) in _jobs )
      {
        if ( IsCancellationRequested )
          return;

        _currentJob = job;
        _currentJobKey = jobKey;

        await job.Execute();

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

    protected abstract Task CreateSubJobs();

    protected virtual Task OnSubJobCompleted( int jobKey, IJob job ) => Task.CompletedTask;

    protected void AddJob( int jobKey, IJob job )
    {
      _jobs.Add( (jobKey, job) );
    }

    #endregion

  }

}
