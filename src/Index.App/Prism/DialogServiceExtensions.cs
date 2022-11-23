using System;
using Index.App.Views;
using Prism.Services.Dialogs;

namespace Index.App.Prism
{

  public static class DialogServiceExtensions
  {

    public static void ShowUnhandledExceptionDialog( this IDialogService dialogService, Exception exception, Action<IDialogResult> callback = null )
    {
      var parameters = new DialogParameters();
      parameters.Add( nameof( Exception ), exception );

      dialogService.ShowDialog( nameof( UnhandledExceptionDialog ), parameters, callback );
    }

  }

}
