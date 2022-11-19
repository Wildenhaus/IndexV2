using Index.Domain.FileSystem;
using Index.Domain.GameProfiles;

namespace Index.Domain.Models
{

  public interface IEditorEnvironment
  {

    public string GameId { get; set; }
    public string GameName { get; set; }
    public string GamePath { get; set; }
    public IGameProfile GameProfile { get; set; }

    public IFileSystem FileSystem { get; }

  }

}
