using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.App.Models;
using Index.Domain.Models;
using Index.Domain.ViewModels;

namespace Index.App.ViewModels
{

  public class EditorLoadingViewModel : ViewModelBase
  {

    #region Events

    public event EventHandler? Complete;
    public event EventHandler? Faulted;

    #endregion

    #region Data Members

    private IEditorEnvironment _environment;
    private string _status;

    #endregion

    #region Properties

    public string Status
    {
      get => _status;
      set => SetProperty( ref _status, value );
    }

    #endregion

    #region Constructor

    public EditorLoadingViewModel( IEditorEnvironment editorEnvironment )
    {
      _environment = editorEnvironment;

      _status = "Initializing...";
      Task.Factory.StartNew( Initialize, TaskCreationOptions.LongRunning );
    }

    #endregion

    #region Private Methods

    private async Task Initialize()
    {
      try
      {
        await RunTask( "Initializing FileSystem", () => Task.Delay( 1000 ) );
        await RunTask( "Initializing Database", () => Task.Delay( 2500 ) );

        Complete?.Invoke( this, EventArgs.Empty );
      }
      catch ( Exception ex )
      {
        Faulted?.Invoke( this, EventArgs.Empty );
      }
    }

    private async Task RunTask( string status, Func<Task> taskFactory )
    {
      SetStatus( status );
      await taskFactory();
    }

    private void SetStatus( string status )
      => Status = $"{status}...";

    #endregion

  }

}
