using Index.Jobs;

namespace Index.Domain.Jobs
{

  public class CompletedJob : JobBase
  {

    public CompletedJob()
    {
      State = JobState.Completed;
      InternalSetAsCompleted();
    }

  }

  public class CompletedJob<T> : JobBase<T>
  {

    public CompletedJob( T result )
    {
      State = JobState.Completed;
      SetResult( result );
      InternalSetAsCompleted();
    }

  }

}
