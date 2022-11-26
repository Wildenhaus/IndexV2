using System;
using Index.Domain.Editors;
using Index.Modules.TextureEditor.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.Modules.TextureEditor
{

  public class TextureEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<TextureEditorView>( DefaultEditorKeys.TextureEditor );
    }

  }

}
