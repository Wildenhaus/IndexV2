using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using Index.Jobs;
using Index.UI.ViewModels;

namespace Index.Modules.JobManager.ViewModels
{

  public class JobsViewModel : TabViewModelBase
  {

    private IJobManager _jobManager;
    private readonly object _collectionLock;
    private Dictionary<Guid, JobViewModel> _jobLookup;

    public ObservableCollection<JobViewModel> Jobs { get; }

    public JobsViewModel( IJobManager jobManager )
    {
      TabName = "Jobs";

      _jobManager = jobManager;

      _collectionLock = new object();
      _jobLookup = new Dictionary<Guid, JobViewModel>();
      Jobs = new ObservableCollection<JobViewModel>();
      BindingOperations.EnableCollectionSynchronization( Jobs, _collectionLock );

      _jobManager.JobStarted += OnJobStarted;
      _jobManager.JobCompleted += OnJobCompleted;
    }

    private void OnJobStarted( object? sender, IJob job )
    {
      if ( job.State > JobState.Executing )
        return;

      var model = new JobViewModel( job );
      lock ( _collectionLock )
      {
        Jobs.Add( model );
        _jobLookup.Add( job.Id, model );
      }
    }

    private void OnJobCompleted( object? sender, IJob job )
    {
      lock ( _collectionLock )
      {
        if ( !_jobLookup.TryGetValue( job.Id, out var model ) )
          return;

        Jobs.Remove( model );
        _jobLookup.Remove( job.Id );
      }
    }

  }

}
