using Index.Jobs;

namespace Index.Domain.Jobs
{

  public static class JobExtensions
  {

    public static void RegisterCompletionCallback( this IJob job, Action<IJob> callback )
    {
      Task.WhenAny( job.Completion ).ContinueWith( t =>
      {
        callback( job );
      } );
    }

    public static void RegisterCompletionCallback<TResult>( this IJob<TResult> job, Action<IJob<TResult>> callback )
    {
      Task.WhenAny( job.Completion ).ContinueWith( t =>
      {
        callback( job );
      } );
    }

  }

}
