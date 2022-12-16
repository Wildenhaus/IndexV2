using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Index.Domain.Editors;
using Index.Modules.TextEditor.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.Modules.TextEditor
{

  public class TextEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<TextEditorView>( DefaultEditorKeys.TextEditorKey );
    }

  }

}
