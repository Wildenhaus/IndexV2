﻿using System.Collections.Generic;
using Index.Modules.DataExplorer.ViewModels;
using Index.UI.Common;

namespace Index.Modules.DataExplorer.Services
{

  public class BulkExportAssetNodeViewModelFactory
  {

    #region Public Methods

    public IxObservableCollection<BulkExportAssetNodeViewModel> CreateExportNodes(ICollection<AssetNodeViewModel> assetNodes)
    {
      var exportNodes = new IxObservableCollection<BulkExportAssetNodeViewModel>();

      foreach(var assetNode in assetNodes)
      {
        var exportNode = CreateExportNode( assetNode );
        exportNodes.Add( exportNode );
      }

      return exportNodes;
    }

    public List<BulkExportAssetNodeViewModel> CreateNodeSearchGraph( IEnumerable<BulkExportAssetNodeViewModel> nodes )
    {
      var flattenedList = new List<BulkExportAssetNodeViewModel>();

      void FlattenNode( BulkExportAssetNodeViewModel node, List<BulkExportAssetNodeViewModel> result )
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

    #region Private Methods

    private BulkExportAssetNodeViewModel CreateExportNode( AssetNodeViewModel assetNode)
    {
      var exportNode = new BulkExportAssetNodeViewModel( assetNode );

      exportNode.Children.SuppressNotifications = true;
      foreach(var childAssetNode in assetNode.Children )
      {
        var childExportNode = CreateExportNode( childAssetNode );
        exportNode.AddChild(childExportNode);
      }
      exportNode.Children.SuppressNotifications = false;

      return exportNode;
    }

    #endregion

  }

}
