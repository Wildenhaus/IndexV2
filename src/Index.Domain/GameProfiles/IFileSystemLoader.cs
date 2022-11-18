using Index.Domain.FileSystem;

namespace Index.Domain.GameProfiles
{

  public interface IFileSystemLoader
  {

    void SetBasePath( string basePath );
    Task<IReadOnlyList<IFileSystemDevice>> LoadDevices();

  }

  public interface IFileSystemLoader<TProfile> : IFileSystemLoader
    where TProfile : class, IGameProfile
  {
  }

}
