using System.Windows.Forms;
using Index.Domain.Services;

namespace Index.Modules.FileDialogs
{

  public class FileDialogService : IFileDialogService
  {

    public string? BrowseForDirectory(
      string title = null,
      string defaultPath = null )
    {
      using ( var dialog = new FolderBrowserDialog() )
      {
        if ( !string.IsNullOrWhiteSpace( title ) )
        {
          dialog.Description = title;
          dialog.UseDescriptionForTitle = true;
        }

        if ( !string.IsNullOrWhiteSpace( defaultPath ) )
          dialog.InitialDirectory = defaultPath;

        if ( dialog.ShowDialog() != DialogResult.OK )
          return null;

        return dialog.SelectedPath;
      }
    }

    public string[]? BrowseForOpenFile(
      string title = null,
      string defaultFileName = null,
      string initialDirectory = null,
      string filter = null,
      bool multiselect = true )
    {
      using ( var dialog = new OpenFileDialog() )
      {
        dialog.CheckPathExists = true;
        dialog.Multiselect = multiselect;

        if ( !string.IsNullOrWhiteSpace( title ) )
          dialog.Title = title;

        if ( !string.IsNullOrWhiteSpace( defaultFileName ) )
          dialog.FileName = defaultFileName;

        if ( !string.IsNullOrWhiteSpace( filter ) )
          dialog.Filter = filter;

        if ( !string.IsNullOrWhiteSpace( initialDirectory ) )
          dialog.InitialDirectory = initialDirectory;

        if ( dialog.ShowDialog() != DialogResult.OK )
          return null;

        return dialog.FileNames;
      }
    }

    public string? BrowseForSaveFile(
      string title = null,
      string defaultFileName = null,
      string initialDirectory = null,
      string filter = null )
    {
      using ( var dialog = new SaveFileDialog() )
      {
        dialog.CheckPathExists = true;
        dialog.OverwritePrompt = true;

        if ( !string.IsNullOrWhiteSpace( title ) )
          dialog.Title = title;

        if ( !string.IsNullOrWhiteSpace( defaultFileName ) )
          dialog.FileName = defaultFileName;

        if ( !string.IsNullOrWhiteSpace( initialDirectory ) )
          dialog.InitialDirectory = initialDirectory;

        if ( !string.IsNullOrWhiteSpace( filter ) )
          dialog.Filter = filter;

        if ( dialog.ShowDialog() != DialogResult.OK )
          return null;

        return dialog.FileName;
      }
    }

  }

}
