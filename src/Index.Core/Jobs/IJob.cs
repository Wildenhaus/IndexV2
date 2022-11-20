using System.ComponentModel;

namespace Index.Jobs
{

  public interface IJob : IDisposable, INotifyPropertyChanged
  {

    #region Events

    event EventHandler Cancelled;
    event EventHandler Completed;
    event EventHandler Faulted;
    event EventHandler Initialized;
    event EventHandler Started;

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

    #endregion

    #region Public Methods

    Task Initialize();
    Task Execute();
    void Cancel();

    #endregion

  }

}
