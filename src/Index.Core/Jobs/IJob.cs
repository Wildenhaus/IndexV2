using System.ComponentModel;

namespace Index.Jobs
{

  public interface IJob : IDisposable, INotifyPropertyChanged
  {

    #region Events

    event EventHandler Cancelled;
    event EventHandler Completed;
    event EventHandler Initialized;
    event EventHandler Started;
    event EventHandler<Exception> Faulted;

    #endregion

    #region Properties

    string Name { get; }
    JobState State { get; }
    StatusList StatusList { get; }

    bool IsIndeterminate { get; }
    string Status { get; }

    int CompletedUnits { get; }
    int TotalUnits { get; }
    string UnitName { get; }

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
