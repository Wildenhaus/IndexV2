using System.Collections.ObjectModel;
using Serilog;

namespace Index.Jobs
{

  public class JobManager : IJobManager
  {

    #region Data Members

    private readonly ILogger _logger;
    private readonly object _collectionLock;

    private ObservableCollection<IJob> _jobs;

    #endregion

    #region Properties

    public ReadOnlyObservableCollection<IJob> Jobs { get; }

    #endregion

    #region Constructor

    public JobManager()
    {
      _logger = Log.Logger;
      _collectionLock = new object();

      _jobs = new ObservableCollection<IJob>();
      Jobs = new ReadOnlyObservableCollection<IJob>( _jobs );
    }

    #endregion

    #region Public Methods

    public void StartJob( IJob job )
    {
      ASSERT( job.State == JobState.Pending );

      lock ( _collectionLock )
        _jobs.Add( job );

      Task.Factory.StartNew( job.Execute, TaskCreationOptions.LongRunning );

      job.Completion.ContinueWith( t =>
        {
          //lock ( _collectionLock )
          //  _jobs.Remove( job );
        } );
    }

    #endregion

  }

}
