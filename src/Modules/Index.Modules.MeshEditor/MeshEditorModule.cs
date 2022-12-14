using Index.Domain.Assets.Meshes;
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
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<MeshEditorView>( DefaultEditorKeys.MeshEditorKey );
      containerRegistry.RegisterDialog<MeshAssetExportOptionsView, AssetExportOptionsViewModel<MeshAssetExportOptions>>( nameof( MeshAssetExportOptions ) );
    }

  }

}
