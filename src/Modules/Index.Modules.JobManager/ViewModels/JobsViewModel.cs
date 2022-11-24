using System.Collections.ObjectModel;
using Index.Jobs;
using Index.UI.ViewModels;

namespace Index.Modules.JobManager.ViewModels
{

  public class JobsViewModel : TabViewModelBase
  {

    private IJobManager _jobManager;

    public ReadOnlyObservableCollection<IJob> Jobs => _jobManager.Jobs;

    public JobsViewModel( IJobManager jobManager )
    {
      _jobManager = jobManager;

      TabName = "Jobs";
    }

  }

}
