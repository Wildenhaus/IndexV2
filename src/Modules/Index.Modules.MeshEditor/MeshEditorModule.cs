using Index.Domain.Editors;
using Index.Modules.MeshEditor.Views;
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
    }

  }

}
