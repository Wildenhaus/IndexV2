using System.Collections.ObjectModel;

namespace Index.Jobs
{

  public interface IJobManager
  {

    #region Events

    event EventHandler<IJob> JobStarted;
    event EventHandler<IJob> JobCompleted;

    #endregion

    #region Properties

    IReadOnlyDictionary<Guid, IJob> Jobs { get; }

    #endregion

    #region Public Methods

    TJob CreateJob<TJob>( IParameterCollection parameters = null ) where TJob : class, IJob;

    TJob StartJob<TJob>( Action<IJob> onCompletion = null ) where TJob : class, IJob;
    TJob StartJob<TJob>( IParameterCollection parameters, Action<IJob> onCompletion = null ) where TJob : class, IJob;
    void StartJob( IJob job, Action<IJob> callback = null );
    void CancelJob( IJob job, Action<IJob> onCancelled = null );

    #endregion

  }

}
