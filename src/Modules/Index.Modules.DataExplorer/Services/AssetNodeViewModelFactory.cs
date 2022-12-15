using System.Collections.Generic;
using System.IO;
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
        rootCategories.Add( CreateCategoryNode( referenceCollection ) );

      rootCategories.Sort( ( a, b ) => a.Name.CompareTo( b.Name ) );
      return rootCategories;
    }

    #endregion

    private AssetNodeViewModel CreateCategoryNode( IAssetReferenceCollection assetReferenceCollection )
    {
      var categoryNode = new AssetNodeViewModel( assetReferenceCollection.AssetTypeName );

      var groups = assetReferenceCollection.GroupBy( x => GetAssetSubDirectory( x ) );
      foreach ( var group in groups )
      {
        if ( group.Count() == 1 )
        {
          categoryNode.Children.Add( new AssetNodeViewModel( group.Single() ) );
          continue;
        }

        if ( string.IsNullOrEmpty( group.Key ) )
        {
          foreach ( var asset in group )
            categoryNode.Children.Add( new AssetNodeViewModel( asset ) );

          continue;
        }

        var groupNode = new AssetNodeViewModel( group.Key );
        foreach ( var asset in group )
          groupNode.Children.Add( new AssetNodeViewModel( asset ) );

        categoryNode.Children.Add( groupNode );
      }

      return categoryNode;
    }

    private string GetAssetSubDirectory( IAssetReference assetReference )
    {
      var assetName = assetReference.AssetName;
      return Path.GetDirectoryName( assetName ) ?? string.Empty;
    }

  }

}
