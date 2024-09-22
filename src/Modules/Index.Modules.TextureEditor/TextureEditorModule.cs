using Index.Domain.Assets;
using Index.Domain.Assets.Textures;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Domain.Editors;
using Index.Modules.TextureEditor.Views;
using Index.UI.ViewModels;
using Index.UI.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace Index.Modules.TextureEditor
{

  public class TextureEditorModule : IModule
  {

    public void OnInitialized( IContainerProvider containerProvider )
    {
      var assetManager = containerProvider.Resolve<IAssetManager>();
      assetManager.RegisterViewTypeForExportOptionsType(typeof(DxgiTextureExportOptions), typeof(DxgiTextureAssetExportOptionsView));
    }

    public void RegisterTypes( IContainerRegistry containerRegistry )
    {
      containerRegistry.RegisterForNavigation<TextureEditorView>( DefaultEditorKeys.TextureEditor );

      containerRegistry.RegisterDialog<DxgiTextureAssetExportOptionsView, AssetExportOptionsWindowViewModel<DxgiTextureExportOptions>>( nameof( DxgiTextureExportOptions ) );
    }

  }

}
