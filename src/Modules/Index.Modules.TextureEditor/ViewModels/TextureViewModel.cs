using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Index.UI.ViewModels;
using PropertyChanged;

namespace Index.Modules.TextureEditor.ViewModels
{

  public class TextureViewModel : ViewModelBase
  {

    #region Data Members

    private readonly object _lock;

    #endregion

    #region Properties

    public IEnumerable<(string, string)> TextureInformation { get; set; }

    public ObservableCollection<TextureImageViewModel> Images { get; set; }
    public int ImageCount { get; private set; }

    public TextureImageViewModel SelectedImage { get; set; }

    #endregion

    #region Constructor

    public TextureViewModel()
    {
      _lock = new object();

      Images = new ObservableCollection<TextureImageViewModel>();

      BindingOperations.EnableCollectionSynchronization( Images, _lock );
    }

    #endregion

    #region Private Methods

    internal void AddImage( TextureImageViewModel image )
    {
      lock ( _lock )
      {
        Images.Add( image );
        ImageCount++;
      }
    }

    #endregion

  }

}
