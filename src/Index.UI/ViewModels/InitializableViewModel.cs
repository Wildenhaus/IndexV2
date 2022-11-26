using System.Threading.Tasks;
using Index.Common;

namespace Index.UI.ViewModels
{

  public abstract class InitializableViewModel : ViewModelBase
  {

    #region Data Members

    private bool _isInitialized;

    #endregion

    #region Properties

    public bool IsInitializing { get; private set; }
    public IProgressInfo InitializationProgress { get; protected set; }

    #endregion

    #region Public Methods

    public async Task<StatusList> Initialize()
    {
      if ( _isInitialized )
        return new StatusList();

      IsInitializing = true;
      var statusList = await OnInitializing();
      IsInitializing = false;

      _isInitialized = true;
    }

    #endregion

    #region Abstract Methods

    protected abstract Task<StatusList> OnInitializing();

    #endregion

  }

}
