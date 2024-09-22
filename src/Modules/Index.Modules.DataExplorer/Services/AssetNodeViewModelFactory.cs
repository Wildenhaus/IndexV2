using System;
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

    public List<AssetNodeViewModel> CreateNodeSearchGraph( List<AssetNodeViewModel> nodes )
    {
      var flattenedList = new List<AssetNodeViewModel>();

      void FlattenNode( AssetNodeViewModel node, List<AssetNodeViewModel> result )
      {
        foreach ( var child in node.Children )
        {
          FlattenNode( child, result );
        }

        result.Add( node );
      }

      foreach ( var root in nodes )
      {
        FlattenNode( root, flattenedList );
      }

      return flattenedList;
    }

    #endregion

    private AssetNodeViewModel CreateCategoryNode( IAssetReferenceCollection assetReferenceCollection )
    {
      var categoryNode = new AssetNodeViewModel( assetReferenceCollection.AssetTypeName );
      categoryNode.AssetType = assetReferenceCollection.AssetType;
      categoryNode.Children.SuppressNotifications = true;

      var groups = assetReferenceCollection.Where(x => !x.Node.IsHidden).GroupBy( x => GetAssetSubDirectory( x ) );
      if ( groups.Count() == 1 )
      {
        foreach ( var asset in groups.Single().Where( x => !x.Node.IsHidden ).OrderBy( x => x.Node.Name ) )
          categoryNode.Children.Add( new AssetNodeViewModel( asset ) );

        categoryNode.Children.SuppressNotifications = false;
        return categoryNode;
      }

      foreach ( var group in groups.OrderBy( x => x.Key ) )
      {
        if ( group.Count() == 1 )
        {
          categoryNode.Children.Add( new AssetNodeViewModel( group.Single() ) );
          continue;
        }

        if ( string.IsNullOrEmpty( group.Key ) )
        {
          foreach ( var asset in group.Where(x => !x.Node.IsHidden).OrderBy( x => x.Node.Name ) )
            categoryNode.Children.Add( new AssetNodeViewModel( asset ) );

          continue;
        }

        var groupNode = new AssetNodeViewModel( group.Key );
        foreach ( var asset in group.Where( x => !x.Node.IsHidden ).OrderBy( x => x.Node.Name ) )
          groupNode.Children.Add( new AssetNodeViewModel( asset ) );

        categoryNode.Children.Add( groupNode );
      }

      categoryNode.Children.SuppressNotifications = false;
      return categoryNode;
    }

    private string GetAssetSubDirectory( IAssetReference assetReference )
    {
      var assetName = assetReference.AssetName;
      return Path.GetDirectoryName( assetName ) ?? string.Empty;
    }

  }

}
