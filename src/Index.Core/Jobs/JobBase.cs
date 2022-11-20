using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Index.Jobs
{

  public abstract class JobBase : DisposableObject, IJob
  {

    #region Events

    public event EventHandler? Cancelled;
    public event EventHandler? Completed;
    public event EventHandler? Faulted;
    public event EventHandler? Initialized;
    public event EventHandler? Started;

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    #region Data Members

    private string _name;
    private JobState _state;
    private StatusList _statusList;

    private bool _isIndeterminate;
    private string _status;
    private int _completedUnits;
    private int _totalUnits;
    private string _unitName;

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

    public bool IsIndeterminate
    {
      get => _isIndeterminate;
      protected set => SetProperty( ref _isIndeterminate, value );
    }

    public string Status
    {
      get => _status;
      protected set => SetProperty( ref _status, value );
    }

    public int CompletedUnits
    {
      get => _completedUnits;
      protected set => SetProperty( ref _completedUnits, value );
    }

    public int TotalUnits
    {
      get => _totalUnits;
      protected set => SetProperty( ref _totalUnits, value );
    }

    public string UnitName
    {
      get => _unitName;
      protected set => SetProperty( ref _unitName, value );
    }

    public Task Completion
    {
      get => _executeCompletionSource.Task;
    }

    protected bool IsCancellationRequested
    {
      get => _cancellationTokenSource.IsCancellationRequested;
    }

    #endregion

    #region Constructor

    protected JobBase()
    {
      _isIndeterminate = true;

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

      Status = "Finishing Current Operations";
      IsIndeterminate = true;
    }

    public async Task Execute()
    {
      await Initialize();

      if ( State != JobState.Initialized )
        return;

      try
      {
        State = JobState.Executing;
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
      if ( _state <= JobState.Initialized )
        return;

      if ( _state == JobState.Initializing )
      {
        await _initializationCompletionSource.Task;
        return;
      }

      ASSERT( _state == JobState.Idle );

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
          return;
        }
      }
      catch ( Exception ex )
      {
        State = JobState.Faulted;
        RaiseFaultedEvent();

        StatusList.AddError( "Initialization", ex );
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
    protected void RaiseFaultedEvent() => Faulted?.Invoke( this, EventArgs.Empty );
    protected void RaiseInitializedEvent() => Initialized?.Invoke( this, EventArgs.Empty );
    protected void RaiseStartedEvent() => Started?.Invoke( this, EventArgs.Empty );

    private void HandleException( Exception ex )
    {
      State = JobState.Faulted;
      StatusList.AddError( "Job", ex );

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

    #endregion

    #region Private Methods

    private void SetProperty<T>( ref T field, T value, [CallerMemberName] string propertyName = null )
    {
      field = value;
      PropertyChanged?.Invoke( this, new PropertyChangedEventArgs( propertyName ) );
    }

    #endregion

  }
}
