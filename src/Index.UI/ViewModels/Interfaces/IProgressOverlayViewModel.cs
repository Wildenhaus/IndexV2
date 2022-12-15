using Index.Common;

namespace Index.UI.ViewModels
{

  public interface IProgressOverlayViewModel : IViewModel
  {

    #region Properties

    public bool ShowProgressOverlay { get; }
    public IProgressInfo Progress { get; }

    #endregion

  }

}
