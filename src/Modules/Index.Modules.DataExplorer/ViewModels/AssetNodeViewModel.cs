using Index.Domain.Assets;
using Index.UI.Commands;
using Index.UI.Controls.Menus;
using Prism.Commands;

namespace Index.Modules.DataExplorer.ViewModels
{

  public class AssetNodeViewModel : NodeViewModelBase<AssetNodeViewModel>
  {

    #region Data Members

    private readonly string _name;
    private readonly IAssetReference _assetReference;

    #endregion

    #region Properties

    public override string Name
    {
      get => _name;
    }

    #endregion

    #region Constructor

    public AssetNodeViewModel( IAssetReference assetReference )
    {
      _assetReference = assetReference;
      _name = assetReference.Node.DisplayName;

      DoubleClickCommand = new DelegateCommand( HandleDoubleClick );
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
          item.Command( EditorCommands.OpenTabCommand, _assetReference );
        } )
        .AddSeparator()
        .AddItem( "Properties" )
        .AddItem( "Extract" );
    }

    #endregion

    #region Private Methods

    private void HandleDoubleClick()
      => EditorCommands.OpenTabCommand.Execute( _assetReference );

    #endregion

  }

}
