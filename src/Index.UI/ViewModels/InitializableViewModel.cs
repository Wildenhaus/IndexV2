using Index.Common;
using Index.Jobs;
using Prism.Commands;
using Prism.Ioc;

namespace Index.UI.ViewModels
{

  public abstract class InitializableViewModel : ViewModelBase, IInitializableViewModel
  {

    #region Data Members

    private readonly IContainerProvider _container;
    private readonly IJobManager _jobManager;

    private IJob _initializationJob;

    #endregion

    #region Properties

    public bool ShowProgressOverlay { get; protected set; }
    public IProgressInfo Progress { get; protected set; }

    public bool IsInitializing { get; private set; }
    public bool IsInitialized { get; private set; }
    public DelegateCommand CancelInitializationCommand { get; private set; }

    #endregion

    #region Constructor

    protected InitializableViewModel( IContainerProvider container )
    {
      _container = container;
      _jobManager = container.Resolve<IJobManager>();
    }

    #endregion

    #region Public Methods

    public void Initialize()
    {
      if ( IsInitialized )
        return;

      _initializationJob = CreateInitializationJob( _container );
      ASSERT_NOT_NULL( _initializationJob );

      Progress = _initializationJob.Progress;
      CancelInitializationCommand = new DelegateCommand( CancelInitialization,
        () => !_initializationJob?.IsCancellationRequested ?? false );

      _jobManager.StartJob( _initializationJob, InitializationJobCompleted );
      IsInitializing = true;
      ShowProgressOverlay = true;
    }

    public void CancelInitialization()
    {
      if ( !IsInitializing )
        return;

      _jobManager.CancelJob( _initializationJob );
      CancelInitializationCommand?.RaiseCanExecuteChanged();
    }

    #endregion

    #region Abstract Methods

    protected abstract IJob CreateInitializationJob( IContainerProvider container );
    protected abstract void OnInitializationJobCompleted( IJob job );

    #endregion

    #region Private Methods

    private void InitializationJobCompleted( IJob job )
    {
      if ( job.State == JobState.Cancelled )
        return;

      CancelInitializationCommand = null;
      OnInitializationJobCompleted( job );
      IsInitializing = false;
      IsInitialized = true;
      ShowProgressOverlay = false;
      Progress = null;
      _initializationJob = null;

      ConfigureContextMenu();
    }

    #endregion

  }

}
