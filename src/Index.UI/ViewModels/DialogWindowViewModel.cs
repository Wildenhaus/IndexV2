using Index.UI.Windows;
using Prism.Ioc;

namespace Index.UI.ViewModels
{

  public abstract class DialogWindowViewModel : WindowViewModel
  {

    #region Properties

    protected new IxDialogWindow Window
    {
      get => ( IxDialogWindow ) base.Window;
    }

    #endregion

    #region Constructor

    protected DialogWindowViewModel( IContainerProvider container ) 
      : base( container )
    {
    }

    #endregion

  }

}
