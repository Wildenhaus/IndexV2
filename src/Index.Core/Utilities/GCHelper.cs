using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Index.Utilities
{

  public static class GCHelper
  {

    public static void ForceCollect()
    {
      // WPF has odd GC behavior when it comes to bitmaps and some other
      // resource types. To actually clear them from memory, you need to
      // collect twice, while waiting for the finalizers in between collects.
      // Yes, I know this is hacky.
      //GC.Collect();
      //GC.Collect( 2, GCCollectionMode.Forced );
      //GC.WaitForPendingFinalizers();
      //GC.WaitForFullGCComplete();
      //GC.Collect();
      //GC.Collect( 2, GCCollectionMode.Forced );
    }

  }

}
