using Index.Common;
using Prism.Commands;

namespace Index.UI.ViewModels
{

  public interface IInitializableViewModel : IProgressOverlayViewModel
  {

    #region Properties

    public bool IsInitializing { get; }
    public bool IsInitialized { get; }
    public DelegateCommand CancelInitializationCommand { get; }
    public IProgressInfo Progress { get; }

    #endregion

    #region Public Methods

    public void Initialize();
    public void CancelInitialization();

    #endregion

  }

}
