using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Index.UI.Common;
using Index.UI.Services;

namespace Index.UI.Controls
{

  public partial class IxFileBrowserBox : UserControl
  {

    #region Data Members

    private readonly IFileDialogService _fileDialogService;

    #endregion

    #region Properties

    public static readonly DependencyProperty DialogActionTypeProperty = DependencyProperty.Register(
     nameof( DialogActionType ),
     typeof( FileDialogActionType ),
     typeof( IxFileBrowserBox ),
     new PropertyMetadata( FileDialogActionType.Open ) );

    public FileDialogActionType DialogActionType
    {
      get => ( FileDialogActionType ) GetValue( DialogActionTypeProperty );
      set => SetValue( DialogActionTypeProperty, value );
    }

    public static readonly DependencyProperty DialogPathTypeProperty = DependencyProperty.Register(
      nameof( DialogPathType ),
      typeof( FileDialogPathType ),
      typeof( IxFileBrowserBox ),
      new PropertyMetadata( FileDialogPathType.File ) );

    public FileDialogPathType DialogPathType
    {
      get => ( FileDialogPathType ) GetValue( DialogPathTypeProperty );
      set => SetValue( DialogPathTypeProperty, value );
    }

    public static readonly DependencyProperty DialogTitleProperty = DependencyProperty.Register(
      nameof( DialogTitle ),
      typeof( string ),
      typeof( IxFileBrowserBox ) );

    public string DialogTitle
    {
      get => ( string ) GetValue( DialogTitleProperty );
      set => SetValue( DialogTitleProperty, value );
    }

    public static readonly DependencyProperty PathProperty = DependencyProperty.Register(
      nameof( Path ),
      typeof( string ),
      typeof( IxFileBrowserBox ) );

    public bool IsValidPath
    {
      get => ( bool ) GetValue( IsValidPathProperty );
      set => SetValue( IsValidPathProperty, value );
    }

    public static readonly DependencyProperty IsValidPathProperty = DependencyProperty.Register(
      nameof( IsValidPath ),
      typeof( bool ),
      typeof( IxFileBrowserBox ) );

    public string Path
    {
      get => ( string ) GetValue( PathProperty );
      set => SetValue( PathProperty, value );
    }

    #endregion

    #region Constructor

    public IxFileBrowserBox()
    {
      _fileDialogService = new FileDialogService();

      InitializeComponent();
    }

    #endregion

    #region Private Methods

    private string? BrowseForDirectory()
    {
      return _fileDialogService.BrowseForDirectory( title: DialogTitle );
    }

    private string? BrowseForFile()
    {
      switch ( DialogActionType )
      {
        case FileDialogActionType.Open:
          var result = _fileDialogService.BrowseForOpenFile( title: DialogTitle, multiselect: false );
          return result?.FirstOrDefault();

        case FileDialogActionType.Save:
          return _fileDialogService.BrowseForSaveFile( title: DialogTitle );

        default:
          return null;
      }
    }

    #endregion

    #region Event Handlers

    private void OnBrowseButtonClick( object sender, RoutedEventArgs e )
    {
      string path = null;
      switch ( DialogPathType )
      {
        case FileDialogPathType.File:
          path = BrowseForFile();
          break;

        case FileDialogPathType.Directory:
          path = BrowseForDirectory();
          break;
      }

      if ( string.IsNullOrWhiteSpace( path ) )
        return;

      Path = path;
    }

    private void OnPathChanged( object sender, TextChangedEventArgs e )
    {
      var newPath = Path = PathTextBox.Text;
      IsValidPath = File.Exists( newPath ) || Directory.Exists( newPath );
    }

    #endregion

  }

}
