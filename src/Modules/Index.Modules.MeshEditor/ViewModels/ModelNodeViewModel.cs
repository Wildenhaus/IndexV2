using System.Collections.Generic;
using System.Collections.ObjectModel;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Model;
using HelixToolkit.SharpDX.Core.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using Index.UI.ViewModels;
using PropertyChanged;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class ModelNodeViewModel : ViewModelBase
  {

    #region Data Members

    private static readonly MaterialCore DEFAULT_MATERIAL = new DiffuseMaterial();

    private MaterialCore _material;

    #endregion

    #region Properties

    public SceneNode Node { get; }
    public string Name => Node.Name;
    public ICollection<ModelNodeViewModel> Items { get; }

    [OnChangedMethod( nameof( OnNodeVisibilityChanged ) )]
    public bool IsVisible { get; set; }
    [OnChangedMethod( nameof( OnShowTextureChanged ) )]
    public bool ShowTexture { get; set; }
    [OnChangedMethod( nameof( OnShowWireframeChanged ) )]
    public bool ShowWireframe { get; set; }

    #endregion

    #region Constructor

    public ModelNodeViewModel( SceneNode node )
    {
      Node = node;
      if ( node is MeshNode meshNode )
        _material = meshNode.Material;

      node.Tag = this;
      Items = new ObservableCollection<ModelNodeViewModel>();

      IsVisible = true;
      ShowTexture = true;
      ShowWireframe = false;
    }

    #endregion

    #region Event Handlers

    private void OnNodeVisibilityChanged()
    {
      Node.Visible = IsVisible;
      foreach ( var childNode in Node.Traverse() )
      {
        var model = ( childNode.Tag as ModelNodeViewModel );
        if ( model is null )
          continue;

        model.IsVisible = IsVisible;
      }
    }

    private void OnShowTextureChanged()
    {
      if ( Node is MeshNode meshNode )
      {
        if ( ShowTexture )
          meshNode.Material = _material;
        else
          meshNode.Material = DEFAULT_MATERIAL;
      }
    }

    private void OnShowWireframeChanged()
    {
      if ( Node is MeshNode meshNode )
        meshNode.RenderWireframe = ShowWireframe;
    }

    #endregion

  }

}
