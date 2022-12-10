using System.Windows.Controls;
using System.Windows.Input;
using Index.UI.ViewModels;

namespace Index.UI.ViewModels
{

  public abstract class TabViewModelBase : ViewModelBase, ITabViewModel
  {

    #region Properties

    public string TabName { get; set; }
    public ICommand CloseCommand { get; }

    #endregion

  }

}
