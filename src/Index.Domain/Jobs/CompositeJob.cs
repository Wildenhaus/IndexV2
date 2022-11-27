using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Jobs;

namespace Index.Domain.Jobs
{

  public class CompositeJob : CompositeJobBase
  {

    #region Data Members

    private IEnumerable<IJob> _inputJobs;

    #endregion

    #region Constructor

    private CompositeJob( IEnumerable<IJob> inputJobs )
    {
      _inputJobs = inputJobs;
    }

    public static CompositeJob Create( params IJob[] jobs )
      => new CompositeJob( jobs );

    #endregion

    #region Overrides

    protected override void CreateSubJobs()
    {
      var i = 0;
      foreach ( var inputJob in _inputJobs )
        AddJob( i++, inputJob );
    }

    #endregion

  }

}
