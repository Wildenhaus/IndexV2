using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Index.UI.Controls.Buttons
{

  public class DialogButtonBuilder
  {

    #region Properties

    public ObservableCollection<IxButton> Buttons { get; }

    #endregion

    #region Constructor

    public DialogButtonBuilder( ObservableCollection<IxButton> buttons )
    {
      Buttons = buttons;
    }

    #endregion

    #region Public Methods

    public ButtonBuilder AddButton( HorizontalAlignment alignment = HorizontalAlignment.Right )
    {
      var builder = new ButtonBuilder();

      var button = builder.Button;
      button.HorizontalAlignment = alignment;
      button.VerticalAlignment = VerticalAlignment.Center;

      var dock = Enum.Parse<Dock>( alignment.ToString() );
      DockPanel.SetDock( button, dock );

      Buttons.Add( button );
      return builder;
    }

    #endregion

  }

}
