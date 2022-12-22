using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using HelixToolkit.SharpDX.Core;
using HelixToolkit.SharpDX.Core.Assimp;
using HelixToolkit.SharpDX.Core.Model;
using HelixToolkit.SharpDX.Core.Model.Scene;
using HelixToolkit.Wpf.SharpDX;
using Index.Domain.Assets.Meshes;
using Index.UI.ViewModels;
using Index.Utilities;
using PropertyChanged;
using Serilog;

namespace Index.Modules.MeshEditor.ViewModels
{

  public class SceneViewModel : ViewModelBase
  {

    #region Data Members

    private readonly object _collectionLock = new object();
    private ObservableCollection<ModelNodeViewModel> _nodes;

    private ActionDebouncer _searchDebouncer;

    #endregion

    #region Properties

    public SceneNodeGroupModel3D GroupModel { get; private set; }
    public ICollectionView Nodes { get; private set; }

    [OnChangedMethod( nameof( OnSearchTermChanged ) )]
    public string SearchTerm { get; set; }
    [OnChangedMethod( nameof( OnShowTexturesChanged ) )]
    public bool ShowTextures { get; set; }
    [OnChangedMethod( nameof( OnShowWireframeChanged ) )]
    public bool ShowWireframe { get; set; }

    #endregion

    #region Constructor

    public SceneViewModel()
    {
      _searchDebouncer = new ActionDebouncer( 1000, ApplySearchTerm );
      GroupModel = new SceneNodeGroupModel3D();

      ApplyTransforms();

      ShowTextures = true;
    }

    #endregion

    #region Public Methods

    public void ApplyMeshAsset( IMeshAsset meshAsset )
    {
      using ( var importer = new Importer() )
      {
        var nodes = new ObservableCollection<ModelNodeViewModel>();
        importer.ToHelixToolkitScene( meshAsset.AssimpScene, out var helixScene );

        foreach ( var node in helixScene.Root.Traverse() )
        {
          if ( !( node is MeshNode meshNode ) )
            continue;

          meshNode.CullMode = SharpDX.Direct3D11.CullMode.Back;

          ApplyMaterialToNode( meshAsset, meshNode );

          var nodeViewModel = new ModelNodeViewModel( meshNode );
          if ( meshAsset.LodMeshNames.Contains( meshNode.Name ) )
          {
            nodeViewModel.IsVisible = false;
            nodeViewModel.IsLod = true;
          }
          else if ( meshAsset.VolumeMeshNames.Contains( meshNode.Name ) )
          {
            nodeViewModel.IsVisible = false;
            nodeViewModel.IsVolume = true;
          }


          nodes.Add( nodeViewModel );
        }

        GroupModel.Dispatcher.Invoke( () =>
        {
          GroupModel.AddNode( helixScene.Root );
          GroupModel.SceneNode.ForceUpdateTransformsAndBounds();
          GroupModel.GroupNode.ForceUpdateTransformsAndBounds();
        } );

        InitializeNodeCollection( nodes );
      }
    }

    #endregion

    #region Private Methods

    private void ApplyTransforms()
    {
      var transformGroup = new System.Windows.Media.Media3D.Transform3DGroup();

      var rotTransform = new System.Windows.Media.Media3D.RotateTransform3D();
      rotTransform.Rotation = new System.Windows.Media.Media3D.AxisAngleRotation3D(
        new System.Windows.Media.Media3D.Vector3D( 0, 1, 0 ), -90 );
      transformGroup.Children.Add( rotTransform );

      GroupModel.Transform = transformGroup;
    }

    private void ApplyMaterialToNode( IMeshAsset meshAsset, MeshNode meshNode )
    {
      var nodeMaterial = meshNode.Material as PhongMaterialCore;
      if ( nodeMaterial is null )
        return;

      if ( nodeMaterial.DiffuseMap is null && nodeMaterial.DiffuseMapFilePath is not null )
      {
        if ( !meshAsset.Textures.TryGetValue( nodeMaterial.DiffuseMapFilePath, out var texture ) )
        {
          Log.Logger.Error( "Failed to load texture {texName}.", nodeMaterial.Name );
          return;
        }

        nodeMaterial.DiffuseMap = TextureModel.Create( texture.Images[ 0 ].PreviewStream );
        nodeMaterial.UVTransform = new UVTransform( 0, 1, -1, 0, 0 );
      }
    }

    private void InitializeNodeCollection( ObservableCollection<ModelNodeViewModel> nodes )
    {
      Dispatcher.Invoke( () =>
      {
        _nodes = nodes;
        BindingOperations.EnableCollectionSynchronization( _nodes, _collectionLock );

        var collectionView = CollectionViewSource.GetDefaultView( _nodes );
        collectionView.SortDescriptions.Add( new SortDescription( nameof( ModelNodeViewModel.Name ), ListSortDirection.Ascending ) );
        collectionView.Filter = ( obj ) =>
        {
          if ( string.IsNullOrEmpty( SearchTerm ) )
            return true;

          var node = obj as ModelNodeViewModel;
          return node.Name.Contains( SearchTerm, System.StringComparison.OrdinalIgnoreCase );
        };

        Nodes = collectionView;
      } );
    }

    private void OnShowTexturesChanged()
    {
      if ( _nodes is null )
        return;

      foreach ( var node in _nodes )
        node.ShowTexture = ShowTextures;
    }

    private void OnShowWireframeChanged()
    {
      foreach ( var node in _nodes )
        node.ShowWireframe = ShowWireframe;
    }

    private void OnSearchTermChanged()
      => _searchDebouncer.Invoke();

    private void ApplySearchTerm()
    {
      Dispatcher.BeginInvoke( () =>
      {
        Nodes.Refresh();
      } );
    }

    #endregion

    #region Overrides

    protected override void OnDisposing()
    {
      base.OnDisposing();
      lock ( _collectionLock )
        if ( _nodes != null )
          _nodes.Clear();

      foreach ( var n in GroupModel.GroupNode.Traverse() )
        n.ForceDispose();
      GroupModel.GroupNode?.ForceDispose();

      foreach ( var n in GroupModel.SceneNode.Traverse() )
        n.ForceDispose();
      GroupModel.SceneNode?.ForceDispose();

      GroupModel.Clear();
      GroupModel.Dispose();
      GroupModel = null;
    }

    #endregion

  }

}
