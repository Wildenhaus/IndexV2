using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Index.UI.Controls.Buttons
{
  
  public class ButtonBuilder
  {

    #region Properties

    public IxButton Button { get; }

    #endregion

    #region Constructor

    public ButtonBuilder()
    {
      Button = new IxButton();
    }

    #endregion

    #region Public Methods

    public ButtonBuilder Content( object content )
    {
      Button.Content = content;
      return this;
    }

    public ButtonBuilder Icon( SegoeIcon icon )
    {
      Button.Icon = icon;
      return this;
    }

    public ButtonBuilder Command( ICommand command, object commandParameter = null )
    {
      Button.Command = command;
      Button.CommandParameter = commandParameter;
      return this;
    }

    public ButtonBuilder CommandParameter( object commandParameter )
    {
      Button.CommandParameter = commandParameter;
      return this;
    }

    #endregion

  }

}
