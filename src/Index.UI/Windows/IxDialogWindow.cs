using System.Collections.Generic;

namespace Index.UI.Windows
{

  public class IxDialogWindow : IxWindow
  {

    private Dictionary<string, object> _parameters;

    public IDictionary<string, object> Parameters => _parameters;

    public IxDialogWindow()
    {
      _parameters = new Dictionary<string, object>();
    }

  }

}
