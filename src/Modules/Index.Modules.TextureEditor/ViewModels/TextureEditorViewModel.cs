using System;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using Index.Domain.Assets.Textures;
using Index.Domain.Assets.Textures.Dxgi;
using Index.Textures;
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
      Texture.TextureInformation = asset.TextureInformation;
      GeneratePreviews( asset );

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

    private void GeneratePreviews( ITextureAsset asset )
    {
      if ( asset.Images is not null )
        return;

      Progress.Status = "Generating Previews";
      Progress.IsIndeterminate = true;
      ShowProgressOverlay = true;

      switch ( asset )
      {
        case IDxgiTextureAsset dxgiTextureAsset:
          GenerateDxgiPreviews( dxgiTextureAsset );
          break;

        default:
          throw new NotImplementedException( $"{asset.GetType().Name} does not support previews." );
      }

      ShowProgressOverlay = false;
    }

    private void GenerateDxgiPreviews( IDxgiTextureAsset asset )
    {
      var dxgiTextureService = Container.Resolve<IDxgiTextureService>();
      var previewStreams = dxgiTextureService.CreateJpegImageStreams( asset.DxgiImage, includeMips: false );

      var images = new List<ITextureAssetImage>();
      for ( var i = 0; i < previewStreams.Length; i++ )
      {
        var previewStream = previewStreams[ i ];
        var image = new TextureAssetImage( i, previewStream );
        images.Add( image );
      }

      asset.SetPreviewImages( images );
    }

  }

}
