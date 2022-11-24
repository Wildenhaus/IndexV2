namespace Index.Jobs
{

  public enum JobState
  {
    Pending = 0,
    Initializing = 1,
    Initialized = 2,
    Executing = 3,
    Completed = 4,
    Faulted = 5,
    Cancelled = 6
  }

}
