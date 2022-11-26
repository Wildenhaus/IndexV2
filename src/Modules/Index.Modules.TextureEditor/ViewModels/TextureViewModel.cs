using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Index.UI.ViewModels;
using PropertyChanged;

namespace Index.Modules.TextureEditor.ViewModels
{

  public class TextureViewModel : ViewModelBase
  {

    private readonly object _lock;

    public ObservableCollection<TextureImageViewModel> Images { get; set; }
    public TextureImageViewModel SelectedImage { get; set; }

    public TextureViewModel()
    {
      _lock = new object();

      Images = new ObservableCollection<TextureImageViewModel>();

      BindingOperations.EnableCollectionSynchronization( Images, _lock );
    }

    internal void AddImage( TextureImageViewModel image )
    {
      lock ( _lock )
        Images.Add( image );
    }

  }

}
