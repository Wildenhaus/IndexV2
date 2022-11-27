using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Documents;
using Index.Jobs;
using Index.UI.ViewModels;

namespace Index.Modules.JobManager.ViewModels
{

  public class JobsViewModel : TabViewModelBase
  {

    private IJobManager _jobManager;

    public IReadOnlyList<IJob> Jobs => _jobManager.Jobs;

    public JobsViewModel( IJobManager jobManager )
    {
      _jobManager = jobManager;

      TabName = "Jobs";
    }

  }

}
