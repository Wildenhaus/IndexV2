using Index.Domain.Models;
using Index.Modules.Logging.Views;
using Index.UI.ViewModels;
using Prism.Ioc;
using Serilog;

namespace Index.App.ViewModels
{

  public class EditorViewModel : WindowViewModel
  {

    #region Properties

    protected IEditorEnvironment EditorEnvironment { get; }

    #endregion

    #region Constructor

    public EditorViewModel( IContainerProvider container, IEditorEnvironment environment )
      : base( container )
    {
      WindowTitle = $"Index | {environment.GameName}";
      EditorEnvironment = environment;
    }

    #endregion

    #region Overrides

    protected override void OnWindowAppeared()
    {
      InitializeBottomTabPanel();
      Log.Logger.Information( "Welcome to Index! ^_^" );
      Log.Logger.Information( "Using Profile '{ProfileName:l}' v{VersionName:l} by {Author:l}",
        EditorEnvironment.GameName,
        EditorEnvironment.GameProfile.Version,
        EditorEnvironment.GameProfile.Author );
    }

    #endregion

    #region Private Methods

    private void InitializeBottomTabPanel()
    {
      RegionManager.RequestNavigate( "BottomTabPanelRegion", "LogView" );
    }

    #endregion

  }

}
