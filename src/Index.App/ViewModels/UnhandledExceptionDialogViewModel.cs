using System;
using Index.UI.ViewModels;
using Prism.Ioc;
using Prism.Services.Dialogs;

namespace Index.App.ViewModels
{

  public class UnhandledExceptionDialogViewModel : DialogWindowViewModel
  {

    #region Properties

    public string? Message { get; protected set; }
    public string? ExceptionText { get; protected set; }

    #endregion

    #region Constructor

    public UnhandledExceptionDialogViewModel( IContainerProvider container )
      : base( container )
    {
      Title = "Error";
    }

    #endregion

    #region Overrides

    public override void OnDialogOpened( IDialogParameters parameters )
    {
      base.OnDialogOpened( parameters );
      var exception = parameters.GetValue<Exception>( nameof( Exception ) );

      Message = exception.Message;
      ExceptionText = exception.ToString();
    }

    #endregion

  }

}
