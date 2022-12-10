using System.Windows.Input;
using Index.Domain;
using Index.Domain.Assets;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

namespace Index.UI.Commands
{

  public static class EditorCommands
  {

    #region Data Members

    private static IContainerProvider _container;

    #endregion

    #region Commands

    public static ICommand NavigateToAssetCommand { get; private set; }

    #endregion

    #region Constructor

    static EditorCommands()
    {
      NavigateToAssetCommand = new DelegateCommand<IAssetReference>( NavigateToAsset );
    }

    #endregion

    #region Public Methods

    public static void Initialize( IContainerProvider container )
    {
      _container = container;
    }

    #endregion

    #region Private Methods

    private static void NavigateToAsset( IAssetReference assetReference )
    {
      var parameters = new NavigationParameters
      {
        { "AssetReference", assetReference },
        { "AssetName", assetReference.AssetName },
        { "AssetType", assetReference.AssetType }
      };

      var regionManager = _container.Resolve<IRegionManager>();
      regionManager.RequestNavigate( RegionKeys.EditorDocumentRegion, assetReference.EditorKey, parameters );
    }

    #endregion

  }

}
