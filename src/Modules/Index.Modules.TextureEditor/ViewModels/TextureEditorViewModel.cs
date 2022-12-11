using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Index.Domain.Assets.Textures;
using Index.Jobs;
using Index.UI.ViewModels;
using Prism.Ioc;

namespace Index.Modules.TextureEditor.ViewModels
{

  public class TextureEditorViewModel : EditorViewModelBase<ITextureAsset>
  {

    #region Properties

    public TextureViewModel Texture { get; set; }

    #endregion

    public TextureEditorViewModel( IContainerProvider container )
      : base( container )
    {
      Texture = new TextureViewModel();
    }

    protected override void OnAssetLoaded( ITextureAsset asset )
    {
      foreach ( var image in asset.Images )
      {
        var imageModel = new TextureImageViewModel();
        imageModel.ImageIndex = image.Index;

        var preview = new BitmapImage();
        preview.BeginInit();
        preview.StreamSource = image.PreviewStream;
        preview.EndInit();
        preview.Freeze();

        imageModel.Preview = preview;

        Texture.AddImage( imageModel );
      }
    }

  }

}
