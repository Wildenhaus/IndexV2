using System.ComponentModel;
using System.Runtime.CompilerServices;
using Index.Common;

namespace Index.Jobs
{

  public abstract class JobBase : DisposableObject, IJob
  {

    #region Events

    public event EventHandler? Cancelled;
    public event EventHandler? Completed;
    public event EventHandler? Initialized;
    public event EventHandler? Started;
    public event EventHandler? Faulted;

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Data Members

    private string _name;
    private JobState _state;
    private StatusList _statusList;
    private IProgressInfo _progress;

    private CancellationTokenSource _cancellationTokenSource;

    private TaskCompletionSource _initializationCompletionSource;
    private TaskCompletionSource _executeCompletionSource;

    #endregion

    #region Properties

    public string Name
    {
      get => _name;
      protected set => SetProperty( ref _name, value );
    }

    public JobState State
    {
      get => _state;
      protected set => SetProperty( ref _state, value );
    }

    public StatusList StatusList
    {
      get => _statusList;
    }

    public IProgressInfo Progress
    {
      get => _progress;
    }

    public Task Completion
    {
      get => _executeCompletionSource.Task;
    }

    public Exception Exception
    {
      get;
      protected set;
    }

    protected CancellationToken CancellationToken
    {
      get => _cancellationTokenSource.Token;
    }

    protected bool IsCancellationRequested
    {
      get => _cancellationTokenSource.IsCancellationRequested;
    }

    #endregion

    #region Constructor

    protected JobBase()
    {
      _progress = new ProgressInfo();
      _statusList = new StatusList();

      _initializationCompletionSource = new TaskCompletionSource();
      _executeCompletionSource = new TaskCompletionSource();
      _cancellationTokenSource = new CancellationTokenSource();
    }

    #endregion

    #region Public Methods

    public void Cancel()
    {
      _cancellationTokenSource.Cancel();

      Progress.Status = "Finishing Current Operations";
      Progress.IsIndeterminate = true;
    }

    public async Task Execute()
    {
      await Initialize();

      if ( State != JobState.Initialized )
        return;

      try
      {
        State = JobState.Executing;
        RaiseStartedEvent();

        await OnExecuting();

        if ( !IsCancellationRequested )
        {
          State = JobState.Completed;
          RaiseCompletedEvent();
        }
        else
          HandleCancellation();
      }
      catch ( Exception ex )
      {
        HandleException( ex );
      }
      finally
      {
        _executeCompletionSource.TrySetResult();
      }
    }

    public async Task Initialize()
    {
      if ( _state >= JobState.Initialized )
        return;

      if ( _state == JobState.Initializing )
      {
        await _initializationCompletionSource.Task;
        return;
      }

      ASSERT( _state == JobState.Pending );

      try
      {
        State = JobState.Initializing;
        await OnInitializing();

        if ( IsCancellationRequested )
        {
          HandleCancellation();
          return;
        }
        else
        {
          State = JobState.Initialized;
          RaiseInitializedEvent();
          return;
        }
      }
      catch ( Exception ex )
      {
        HandleException( ex );
      }
      finally
      {
        _initializationCompletionSource.TrySetResult();
      }
    }

    #endregion

    #region Private Methods

    protected virtual Task OnInitializing() => Task.CompletedTask;
    protected virtual Task OnExecuting() => Task.CompletedTask;
    protected virtual Task OnCompleted() => Task.CompletedTask;

    protected void RaiseCancelledEvent() => Cancelled?.Invoke( this, EventArgs.Empty );
    protected void RaiseCompletedEvent() => Completed?.Invoke( this, EventArgs.Empty );
    protected void RaiseInitializedEvent() => Initialized?.Invoke( this, EventArgs.Empty );
    protected void RaiseStartedEvent() => Started?.Invoke( this, EventArgs.Empty );
    protected void RaiseFaultedEvent() => Faulted?.Invoke( this, EventArgs.Empty );

    protected void HandleException( Exception exception )
    {
      Exception = exception;
      State = JobState.Faulted;
      StatusList.AddError( "Job", exception );

      _initializationCompletionSource.TrySetResult();
      _executeCompletionSource.TrySetResult();

      RaiseFaultedEvent();
    }

    private void HandleCancellation()
    {
      State = JobState.Cancelled;
      StatusList.AddMessage( "Job", "Job has been cancelled by the user." );

      _initializationCompletionSource.TrySetResult();
      _executeCompletionSource.TrySetResult();

      RaiseCancelledEvent();
    }

    private void SetProperty<T>( ref T field, T value, [CallerMemberName] string propertyName = null )
    {
      field = value;
      PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }

    #endregion

  }

  public abstract class JobBase<TResult> : JobBase, IJob<TResult>
  {

    #region Properties

    public TResult? Result { get; protected set; }

    #endregion

  }

}
