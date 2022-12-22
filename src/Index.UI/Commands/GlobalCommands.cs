using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Index.Domain.Assets;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace Index.UI.Commands
{

  public class GlobalCommands
  {

    #region Data Members

    private static IContainerProvider _container;

    #endregion

    #region Commands

    public static ICommand OpenAboutDialogCommand { get; private set; }
    public static ICommand OpenWebPageCommand { get; private set; }

    #endregion

    #region Public Methods

    public static void Initialize( IContainerProvider container )
    {
      _container = container;

      OpenAboutDialogCommand = new DelegateCommand( OpenAboutDialog );
      OpenWebPageCommand = new DelegateCommand<string>( OpenWebPage );
    }

    #endregion

    #region Command Methods

    private static void OpenAboutDialog()
    {
      var dialogService = _container.Resolve<IDialogService>();
      dialogService.ShowDialog( "AboutView" );
    }

    private static void OpenWebPage( string url )
      => Process.Start( new ProcessStartInfo( url ) { UseShellExecute = true } );

    #endregion

  }

}
