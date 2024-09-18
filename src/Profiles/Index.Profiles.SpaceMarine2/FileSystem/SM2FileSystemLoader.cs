using Index.Domain.FileSystem;

namespace Index.Profiles.SpaceMarine2.FileSystem
{

  public class SM2FileSystemLoader : FileSystemLoaderBase
  {
    protected override async Task OnLoadDevices()
    {
      await LoadFilesWithExtension( ".pak", path => new SM2PckDevice( BasePath, path ) );
      //await LoadFileWithName( "default_tpl_0.pak", path => new SM2PckDevice(BasePath, path));
    }
  }

}
