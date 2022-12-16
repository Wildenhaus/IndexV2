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

    public static CompletedJob<T> FromResult<T>( T result )
      => new CompletedJob<T>( result );

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
