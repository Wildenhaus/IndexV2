using System.Windows.Input;
using Index.Domain.Assets;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;

namespace Index.UI.Commands
{

  public static class EditorCommands
  {

    private static IContainerProvider _container;

    public static ICommand NavigateToAssetCommand { get; private set; }

    static EditorCommands()
    {
      NavigateToAssetCommand = new DelegateCommand<IAssetReference>( NavigateToAsset );
    }

    public static void Initialize( IContainerProvider container )
    {
      _container = container;
    }

    private static void NavigateToAsset( IAssetReference assetReference )
    {
      var parameters = new NavigationParameters
      {
        { "AssetReference", assetReference },
        { "AssetName", assetReference.AssetName },
        { "AssetType", assetReference.AssetType }
      };

      var regionManager = _container.Resolve<IRegionManager>();
      regionManager.RequestNavigate( "EditorRegion", assetReference.EditorKey, parameters );
    }

  }

}
