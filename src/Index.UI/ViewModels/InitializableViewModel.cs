using System.Threading;
using System.Threading.Tasks;
using Index.Common;

namespace Index.UI.ViewModels
{

  public abstract class InitializableViewModel : ViewModelBase, IInitializableViewModel
  {

    #region Data Members

    private bool _isInitialized;
    private Task _initializeTask;
    private CancellationTokenSource _initializeCts;

    #endregion

    #region Properties

    public bool IsInitializing { get; private set; }
    public IProgressInfo InitializationProgress { get; protected set; }

    #endregion

    #region Public Methods

    public void Initialize()
    {
      if ( _isInitialized )
        return;

      IsInitializing = true;
      _initializeTask = Task.Factory.StartNew( OnInitializing, TaskCreationOptions.LongRunning );

      _initializeTask.ContinueWith( t =>
      {
        IsInitializing = false;

        if ( t.IsCompletedSuccessfully )
          _isInitialized = true;
      } );
    }

    public void CancelInitialization()
      => _initializeCts.Cancel();

    #endregion

    #region Abstract Methods

    protected abstract Task<StatusList> OnInitializing();

    #endregion

  }

}
