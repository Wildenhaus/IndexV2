using System.Collections.Generic;
using System.Linq;
using Index.Domain.Assets;
using Index.Modules.DataExplorer.ViewModels;

namespace Index.Modules.DataExplorer.Services
{

  public class AssetNodeViewModelFactory
  {

    #region Data Members

    private readonly IAssetManager _assetManager;

    #endregion

    #region Constructor

    public AssetNodeViewModelFactory( IAssetManager assetManager )
    {
      _assetManager = assetManager;
    }

    #endregion

    #region Public Methods

    public List<AssetNodeViewModel> CreateNodes()
    {
      var categories = new List<AssetNodeViewModel>();

      foreach ( var referenceCollection in _assetManager.ReferenceCollections )
      {
        var categoryNode = new AssetNodeViewModel( referenceCollection.AssetTypeName );
        categories.Add( categoryNode );

        foreach ( var assetReference in referenceCollection.OrderBy( x => x.AssetName ) )
          categoryNode.Children.Add( new AssetNodeViewModel( assetReference ) );
      }

      categories.Sort( ( a, b ) => a.Name.CompareTo( b.Name ) );
      return categories;
    }

    #endregion

  }

}
