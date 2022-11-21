using Index.Domain.Models;
using Prism.Mvvm;

namespace Index.App.ViewModels
{

  public class EditorViewModel : BindableBase
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
