using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using Prism.Mvvm;

namespace Index.UI.Controls.Menus
{

  public class MenuItemViewModel : BindableBase, IMenuItemViewModel
  {

    #region Data Members

    private string _header;
    private ICommand _command;
    private object _commandParameter;
    private ImageSource _imageSource;
    private IList<IMenuItemViewModel> _children;

    #endregion

    #region Properties

    public string Header
    {
      get => _header;
      set => SetProperty( ref _header, value );
    }

    public ICommand Command
    {
      get => _command;
      set => SetProperty( ref _command, value );
    }

    public object CommandParameter
    {
      get => _commandParameter;
      set => SetProperty( ref _commandParameter, value );
    }

    public ImageSource ImageSource
    {
      get => _imageSource;
      set => SetProperty( ref _imageSource, value );
    }

    public IList<IMenuItemViewModel> Children
    {
      get => _children;
      set => SetProperty( ref _children, value );
    }

    #endregion

  }

}
