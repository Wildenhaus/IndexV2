using System.Collections.ObjectModel;
using System.Windows.Input;
using Index.UI.Controls;
using Prism.Services.Dialogs;

namespace Index.UI.ViewModels
{

  public interface IDialogWindowViewModel : IWindowViewModel, IDialogAware
  {

    #region Properties

    public ObservableCollection<IxButton> Buttons { get; }
    public ICommand CloseDialogCommand { get; }

    public IDialogParameters Parameters { get; }

    #endregion

  }

}
