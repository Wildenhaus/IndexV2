namespace Index.Domain.FileSystem
{

  public interface IFileSystemLoader
  {

    event Action<double> ProgressChanged;

    void SetBasePath( string basePath );
    Task<IReadOnlyList<IFileSystemDevice>> LoadDevices();

  }

}
