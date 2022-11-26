using Index.Common;

namespace Index.UI.ViewModels
{

  public interface IInitializableViewModel : IViewModel
  {

    #region Properties

    public bool IsInitializing { get; }
    public IProgressInfo InitializationProgress { get; }

    #endregion

    #region Public Methods

    public void Initialize();
    public void CancelInitialization();

    #endregion

  }

}
