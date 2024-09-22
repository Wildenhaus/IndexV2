using Index.Domain.Assets;
using Index.Domain.Assets.Text;
using Index.Domain.Editors;
using Index.Modules.TextEditor.Views;
using Index.UI.ViewModels;
using Index.UI.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.Modules.TextEditor
{

  public class TextEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
      var assetManager = containerProvider.Resolve<IAssetManager>();
      assetManager.RegisterViewTypeForExportOptionsType( typeof( TextAssetExportOptions ), typeof( AssetExportOptionsViewBase ) );
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<TextEditorView>( DefaultEditorKeys.TextEditorKey );

      containerRegistry.RegisterDialog<AssetExportOptionsViewBase, AssetExportOptionsWindowViewModel<TextAssetExportOptions>>( nameof( TextAssetExportOptions ) );
    }

  }

}
