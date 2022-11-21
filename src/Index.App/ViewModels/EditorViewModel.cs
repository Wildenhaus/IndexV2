using Index.Domain.Models;
using Index.Domain.ViewModels;
using Serilog;

namespace Index.App.ViewModels
{

  public class EditorViewModel : ViewModelBase
  {

    #region Properties

    public string WindowTitle
    {
      get;
      set;
    }

    #endregion

    #region Constructor

    public EditorViewModel( IEditorEnvironment environment )
    {
      WindowTitle = $"Index | {environment.GameName}";
    }

    #endregion

  }

}
