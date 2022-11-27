using Index.Common;
using Index.Jobs;

namespace Index.Domain.Jobs
{

  public class DelegateJob : JobBase
  {

    #region Data Members

    private readonly Func<IProgressInfo, Task> _executeTaskFactory;

    #endregion

    #region Constructor

    public DelegateJob( Func<Task> onExecute )
      => _executeTaskFactory = Wrap( onExecute );

    public DelegateJob( Func<IProgressInfo, Task> onExecute )
      => _executeTaskFactory = onExecute;

    public DelegateJob( Action onExecute )
      => _executeTaskFactory = Wrap( onExecute );

    public DelegateJob( Action<IProgressInfo> onExecute )
      => _executeTaskFactory = Wrap( onExecute );

    #endregion

    #region Overrides

    protected override Task OnExecuting()
      => _executeTaskFactory( Progress );

    #endregion

    #region Private Methods

    private Func<IProgressInfo, Task> Wrap( Func<Task> taskFactory )
      => ( progress ) => Task.Run( taskFactory, CancellationToken );

    private Func<IProgressInfo, Task> Wrap( Action action )
      => ( progress ) => Task.Run( action, CancellationToken );

    private Func<IProgressInfo, Task> Wrap( Action<IProgressInfo> action )
      => ( progress ) => Task.Run( () => action( progress ), CancellationToken );

    #endregion

  }

  public class DelegateJob<TResult> : JobBase<TResult>
  {

    #region Data Members

    private readonly Func<IProgressInfo, Task<TResult>> _executeTaskFactory;

    #endregion

    #region Constructor

    public DelegateJob( Func<Task<TResult>> onExecute )
      => _executeTaskFactory = Wrap( onExecute );

    public DelegateJob( Func<IProgressInfo, Task<TResult>> onExecute )
      => _executeTaskFactory = onExecute;

    public DelegateJob( Func<TResult> onExecute )
      => _executeTaskFactory = Wrap( onExecute );

    public DelegateJob( Func<IProgressInfo, TResult> onExecute )
      => _executeTaskFactory = Wrap( onExecute );

    #endregion

    #region Overrides

    protected override async Task OnExecuting()
      => Result = await _executeTaskFactory( Progress );

    #endregion

    #region Private Methods

    private Func<IProgressInfo, Task<TResult>> Wrap( Func<Task<TResult>> taskFactory )
      => ( progress ) => Task.Run( taskFactory, CancellationToken );

    private Func<IProgressInfo, Task<TResult>> Wrap( Func<TResult> func )
      => ( progress ) => Task.Run( func, CancellationToken );

    private Func<IProgressInfo, Task<TResult>> Wrap( Func<IProgressInfo, TResult> func )
      => ( progress ) => Task.Run( () => func( progress ), CancellationToken );

    #endregion

  }

}
