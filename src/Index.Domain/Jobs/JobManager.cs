using System.Collections.ObjectModel;
using Prism.Ioc;
using Serilog;

namespace Index.Jobs
{

  public class JobManager : IJobManager
  {

    #region Data Members

    private readonly IContainerProvider _container;
    private readonly ILogger _logger;

    private readonly object _collectionLock;
    private List<IJob> _jobs;

    #endregion

    #region Properties

    public IReadOnlyList<IJob> Jobs
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

      _jobs = new List<IJob>();
    }

    #endregion

    #region Public Methods

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
      ASSERT( job.State == JobState.Pending );

      lock ( _collectionLock )
        _jobs.Add( job );

      Task.Factory.StartNew( job.Execute, TaskCreationOptions.LongRunning );

      job.Completion.ContinueWith( t =>
      {
        if ( onCompletion is not null )
          onCompletion( job );

        lock ( _collectionLock )
          _jobs.Remove( job );
      } );
    }

    public void CancelJob( IJob job, Action<IJob> onCancelled = null )
    {
      if ( job.State == JobState.Pending )
        return;
      if ( job.State > JobState.Executing )
        return;

      job.Completion.ContinueWith( t =>
      {
        if ( onCancelled is not null )
          onCancelled( job );

        lock ( _collectionLock )
          _jobs.Remove( job );
      } );

      job.Cancel();
    }



    #endregion

  }

}
