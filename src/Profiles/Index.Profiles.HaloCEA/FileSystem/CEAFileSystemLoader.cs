using Index.Domain.FileSystem;

namespace Index.Profiles.HaloCEA.FileSystem
{

  public class CEAFileSystemLoader : FileSystemLoaderBase
  {

    protected override async Task OnLoadDevices()
    {
      await LoadFilesWithExtension( ".s3dpak", path => new S3DPakDevice( path ) );
      await LoadFilesWithExtension( ".ipak", path => new IPakDevice( path ) );
    }

  }

}
