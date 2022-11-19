namespace Index.Domain.FileSystem
{

  public interface IFileSystemLoader
  {

    void SetBasePath( string basePath );
    Task<IReadOnlyList<IFileSystemDevice>> LoadDevices();

  }

}
