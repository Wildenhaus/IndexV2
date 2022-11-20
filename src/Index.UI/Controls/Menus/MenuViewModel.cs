using System.Collections.Generic;

namespace Index.UI.Controls.Menus
{

  public class MenuViewModel
  {

    #region Data Members

    private IList<IMenuItemViewModel> _items;

    #endregion

    #region Properties

    public IList<IMenuItemViewModel> Items
    {
      get => _items;
    }

    #endregion

    #region Constructor

    public MenuViewModel()
    {
      _items = new List<IMenuItemViewModel>();
    }

    #endregion

  }

}
