using System.Collections.ObjectModel;

namespace Index.Jobs
{

  public interface IJobManager
  {

    ReadOnlyObservableCollection<IJob> Jobs { get; }

    void StartJob( IJob job );

  }

}
