using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Index.UI.Controls;
using Index.UI.Controls.Buttons;
using Index.UI.Windows;
using Prism.Commands;
using Prism.Ioc;
using Prism.Services.Dialogs;
using PropertyChanged;

namespace Index.UI.ViewModels
{

  public abstract class DialogWindowViewModel : WindowViewModel, IDialogWindowViewModel
  {

    #region Events

    public event Action<IDialogResult>? RequestClose;

    #endregion

    #region Properties

    public ObservableCollection<IxButton> Buttons { get; }
    public ICommand CloseDialogCommand { get; set; }

    [DoNotNotify]
    public IDialogParameters Parameters { get; private set; }

    [DoNotNotify]
    protected new IxDialogWindow Window => ( IxDialogWindow ) base.Window;

    #endregion

    #region Constructor

    protected DialogWindowViewModel( IContainerProvider container )
      : base( container )
    {
      Buttons = new ObservableCollection<IxButton>();
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
      Parameters = parameters;

      var buttonBuilder = new DialogButtonBuilder( Buttons );
      OnConfigureButtons( buttonBuilder );
    }

    #endregion

    #region Virtual Methods

    protected virtual void OnConfigureButtons( DialogButtonBuilder builder )
    {
      builder.AddButton().Content( "Close" ).Command( CloseDialogCommand );
      builder.AddButton().Content( "Close" ).Command( CloseDialogCommand );
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
