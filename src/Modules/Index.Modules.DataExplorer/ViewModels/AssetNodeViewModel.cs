using Index.Domain.Assets;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class AssetNodeViewModel : NodeViewModelBase<AssetNodeViewModel>
  {

    #region Data Members

    private readonly string _name;
    private readonly IAssetReference _assetReference;

    #endregion

    #region Properties

    public string Name
    {
      get => _name;
    }

    #endregion

    #region Constructor

    public AssetNodeViewModel( IAssetReference assetReference )
    {
      _assetReference = assetReference;
      _name = assetReference.AssetName;
    }

    public AssetNodeViewModel( string assetTypeName )
    {
      _name = assetTypeName;
    }

    #endregion

  }

}
