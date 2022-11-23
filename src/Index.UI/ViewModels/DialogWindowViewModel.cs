using System;
using System.Windows.Input;
using Index.UI.Windows;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  public abstract class DialogWindowViewModel : WindowViewModel, IDialogAware
  {

    #region Events

    public event Action<IDialogResult>? RequestClose;

    #endregion

    #region Properties

    public virtual ICommand CloseDialogCommand { get; }

    [DoNotNotify]
    protected new IxDialogWindow Window => ( IxDialogWindow ) base.Window;

    #endregion

    #region Constructor

    protected DialogWindowViewModel( IContainerProvider container ) 
      : base( container )
    {
      CloseDialogCommand = new DelegateCommand( CloseDialog );
    }

    #endregion

    #region IDialogAware Methods

    public virtual bool CanCloseDialog()
      => true;

    public virtual void OnDialogClosed()
    {
    }

    public virtual void OnDialogOpened( IDialogParameters parameters )
    {
    }

    #endregion

    #region Private Methods

    private void CloseDialog()
      => RaiseRequestClose( new DialogResult() );

    private void RaiseRequestClose( IDialogResult dialogResult )
      => RequestClose?.Invoke( dialogResult );

    #endregion

  }

}
