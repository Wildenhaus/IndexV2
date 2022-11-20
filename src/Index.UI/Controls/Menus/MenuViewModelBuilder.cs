using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;

namespace Index.UI.Controls.Menus
{

  public class MenuViewModelBuilder
  {

    #region Properties

    public IList<IMenuItemViewModel> Items { get; }

    #endregion

    #region Constructor

    public MenuViewModelBuilder()
    {
      Items = new List<IMenuItemViewModel>();
    }

    #endregion

    #region Public Methods

    public MenuViewModelBuilder AddItem( string header, Action<MenuItemViewModelBuilder> configure = null )
    {
      var builder = new MenuItemViewModelBuilder();
      builder.Header( header );

      if ( configure is not null )
        configure( builder );

      Items.Add( builder.Item );
      return this;
    }

    public MenuViewModelBuilder AddItem( Action<MenuItemViewModelBuilder> configure )
    {
      var builder = new MenuItemViewModelBuilder();
      configure( builder );

      Items.Add( builder.Item );
      return this;
    }

    public MenuViewModelBuilder AddSeparator()
    {
      Items.Add( new MenuSeparatorViewModel() );
      return this;
    }

    #endregion

  }

  public class MenuItemViewModelBuilder
  {

    #region Properties

    public MenuItemViewModel Item { get; }

    #endregion

    #region Constructor

    public MenuItemViewModelBuilder()
    {
      Item = new MenuItemViewModel();
    }

    #endregion

    #region Public Methods

    public MenuItemViewModelBuilder Header( string header )
    {
      Item.Header = header;
      return this;
    }

    public MenuItemViewModelBuilder Command( ICommand command, object commandParameter = null )
    {
      Item.Command = command;
      Item.CommandParameter = commandParameter;
      return this;
    }

    public MenuItemViewModelBuilder Image( ImageSource imageSource )
    {
      Item.ImageSource = imageSource;
      return this;
    }

    public MenuItemViewModelBuilder Items( Action<MenuViewModelBuilder> configure )
    {
      var builder = new MenuViewModelBuilder();
      configure( builder );

      Item.Children = builder.Items;
      return this;
    }

    #endregion

  }

}
