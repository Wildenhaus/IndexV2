using Index.Common;
using Prism.Commands;

namespace Index.UI.ViewModels
{

  public interface IInitializableViewModel : IViewModel
  {

    #region Properties

    public bool IsInitializing { get; }
    public DelegateCommand CancelInitializationCommand { get; }
    public IProgressInfo InitializationProgress { get; }

    #endregion

    #region Public Methods

    public void Initialize();
    public void CancelInitialization();

    #endregion

  }

}
