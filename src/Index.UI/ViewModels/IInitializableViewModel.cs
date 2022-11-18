using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.UI.ViewModels
{

  public interface IInitializableViewModel
  {

    bool IsInitializing { get; }

    Task<IResult> Initialize();

  }

}
