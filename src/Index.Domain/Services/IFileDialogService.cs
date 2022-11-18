namespace Index.Domain.Services
{

  public interface IFileDialogService
  {

    string? BrowseForDirectory(
      string title = null,
      string defaultPath = null );

    string[]? BrowseForOpenFile(
      string title = null,
      string defaultFileName = null,
      string initialDirectory = null,
      string filter = null,
      bool multiselect = true );

    string? BrowseForSaveFile(
      string title = null,
      string defaultFileName = null,
      string initialDirectory = null,
      string filter = null );

  }

}
