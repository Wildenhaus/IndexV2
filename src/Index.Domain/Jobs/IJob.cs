using System.ComponentModel;
using Index.Common;

namespace Index.Jobs
{

  public interface IJob : IDisposable, INotifyPropertyChanged
  {

    #region Events

    event EventHandler Cancelled;
    event EventHandler Completed;
    event EventHandler Initialized;
    event EventHandler Started;
    event EventHandler Faulted;

    #endregion

    #region Properties

    string Name { get; }
    JobState State { get; }
    StatusList StatusList { get; }
    IProgressInfo Progress { get; }

    Task Completion { get; }
    Exception Exception { get; }

    #endregion

    #region Public Methods

    Task Initialize();
    Task Execute();
    void Cancel();

    #endregion

  }

  public interface IJob<TResult> : IJob
  {

    #region Properties

    TResult? Result { get; }

    #endregion

  }

}
