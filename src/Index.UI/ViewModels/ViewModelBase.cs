using System.ComponentModel;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  [AddINotifyPropertyChangedInterface]
  public abstract class ViewModelBase : INotifyPropertyChanged
  {

    #region Events

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

  }

}