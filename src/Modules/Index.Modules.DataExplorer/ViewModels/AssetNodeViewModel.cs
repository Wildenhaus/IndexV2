using Index.Domain.Assets;
using Index.UI.Commands;
using Index.UI.Controls.Menus;

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

    #region Overrides

    protected override void OnConfigureContextMenu( MenuViewModelBuilder builder )
    {
      builder
        .AddItem( "Open", item =>
        {
          item.Command( EditorCommands.NavigateToAssetCommand, _assetReference );
        } )
        .AddSeparator()
        .AddItem( "Properties" )
        .AddItem( "Extract" );
    }

    #endregion

  }

}
