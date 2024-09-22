using Index.Domain.Assets;
using Index.Domain.Assets.Meshes;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Domain.Editors;
using Index.Modules.MeshEditor.Views;
using Index.UI.ViewModels;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.Modules.MeshEditor
{

  public class MeshEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
      var assetManager = containerProvider.Resolve<IAssetManager>();
      assetManager.RegisterViewTypeForExportOptionsType( typeof( MeshAssetExportOptions ), typeof( MeshAssetExportOptionsView ) );
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<MeshEditorView>( DefaultEditorKeys.MeshEditorKey );
      containerRegistry.RegisterDialog<MeshAssetExportOptionsView, AssetExportOptionsWindowViewModel<MeshAssetExportOptions>>( nameof( MeshAssetExportOptions ) );
    }

  }

}
