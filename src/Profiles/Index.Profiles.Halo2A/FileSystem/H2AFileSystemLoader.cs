using Index.Domain.FileSystem;

namespace Index.Profiles.Halo2A.FileSystem
{

  public class H2AFileSystemLoader : FileSystemLoaderBase
  {

    #region Overrides

    protected override async Task OnLoadDevices()
    {
      await LoadFilesWithExtension( ".pck", path => new PckDevice( path ) );
    }

    #endregion

  }

}
