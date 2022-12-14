using Prism.Ioc;
using Serilog;

namespace Index.Jobs
{

  public class JobManager : IJobManager
  {

    #region Events

    public event EventHandler<IJob> JobStarted;
    public event EventHandler<IJob> JobCompleted;

    #endregion

    #region Data Members

    private readonly IContainerProvider _container;
    private readonly ILogger _logger;

    private readonly object _collectionLock;
    private Dictionary<Guid, IJob> _jobs;

    #endregion

    #region Properties

    public IReadOnlyDictionary<Guid, IJob> Jobs
    {
      get => _jobs;
    }

    #endregion

    #region Constructor

    public JobManager( IContainerProvider container )
    {
      _container = container;
      _logger = Log.Logger;
      _collectionLock = new object();

      _jobs = new Dictionary<Guid, IJob>();
    }

    #endregion

    #region Public Methods

    public IJob CreateJob( Type jobType, IParameterCollection parameters = null )
    {
      return ( IJob ) Activator.CreateInstance( jobType,
        new object[] { _container, parameters } );
    }

    public TJob CreateJob<TJob>( IParameterCollection parameters = null )
      where TJob : class, IJob
    {
      return ( TJob ) Activator.CreateInstance( typeof( TJob ),
        new object[] { _container, parameters } );
    }

    public TJob StartJob<TJob>( Action<IJob> onCompletion = null )
      where TJob : class, IJob
      => StartJob<TJob>( null, onCompletion );

    public TJob StartJob<TJob>( IParameterCollection parameters, Action<IJob> onCompletion = null )
      where TJob : class, IJob
    {
      var job = CreateJob<TJob>( parameters );

      StartJob( job, onCompletion );
      return job;
    }

    public void StartJob( IJob job, Action<IJob> onCompletion = null )
    {
      lock ( _collectionLock )
      {
        // If the job is already running, just create a completion callback.
        if ( _jobs.TryGetValue( job.Id, out var existingJob ) )
        {
          if ( onCompletion is not null )
            Task.WhenAny( job.Completion ).ContinueWith( t => onCompletion( job ) );
          return;
        }

        // Otherwise, start the job
        Task.Factory.StartNew( job.Execute, TaskCreationOptions.LongRunning );
        _jobs.Add( job.Id, job );
        RaiseJobStarted( job );

        Task.WhenAny( job.Completion ).ContinueWith( t =>
        {
          RaiseJobCompleted( job );
          lock ( _collectionLock )
            _jobs.Remove( job.Id );

          if ( onCompletion is not null )
            onCompletion( job );
        } );
      }
    }

    public void CancelJob( IJob job, Action<IJob> onCancelled = null )
    {
      if ( job.State == JobState.Pending )
        return;
      if ( job.State > JobState.Executing )
        return;

      Task.WhenAny( job.Completion ).ContinueWith( t =>
      {
        if ( onCancelled is not null )
          onCancelled( job );

        lock ( _collectionLock )
          _jobs.Remove( job.Id );
      } );

      job.Cancel();
    }



    #endregion

    #region Private Methods

    private void RaiseJobStarted( IJob job )
      => JobStarted?.Invoke( this, job );

    private void RaiseJobCompleted( IJob job )
      => JobCompleted?.Invoke( this, job );

    #endregion

  }

}
