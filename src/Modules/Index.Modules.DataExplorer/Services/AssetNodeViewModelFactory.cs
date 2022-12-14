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
      var rootCategories = new List<AssetNodeViewModel>();

      foreach ( var referenceCollection in _assetManager.ReferenceCollections )
      {
        var categoryNode = new AssetNodeViewModel( referenceCollection.AssetTypeName );
        rootCategories.Add( categoryNode );

        var subDirectoryLookup = new Dictionary<string, AssetNodeViewModel>();
        foreach ( var assetReference in referenceCollection.OrderBy( x => x.AssetName ) )
        {
          var assetName = assetReference.AssetName;
          var subDirectoryIndex = assetName.IndexOf( '/' );
          if ( subDirectoryIndex != -1 )
          {
            var subDirectoryName = assetName.Substring( 0, subDirectoryIndex );
            if ( !subDirectoryLookup.TryGetValue( subDirectoryName, out var subDirectoryNode ) )
            {
              subDirectoryNode = new AssetNodeViewModel( subDirectoryName );
              categoryNode.Children.Add( subDirectoryNode );
              subDirectoryLookup.Add( subDirectoryName, subDirectoryNode );
            }
            subDirectoryNode.Children.Add( new AssetNodeViewModel( assetReference ) );
          }
          else
            categoryNode.Children.Add( new AssetNodeViewModel( assetReference ) );

        }
      }

      rootCategories.Sort( ( a, b ) => a.Name.CompareTo( b.Name ) );
      return rootCategories;
    }

    #endregion

  }

}
