using Index.Domain.FileSystem;
using Index.Domain.Models;

namespace Index.Domain.GameProfiles
{

  public interface IGameProfile
  {

    #region Properties

    public string GameId { get; }
    public string GameName { get; }

    public string Author { get; }
    public Version Version { get; }

    public IFileSystemLoader FileSystemLoader { get; }
    public IGamePathIdentificationRule IdentificationRule { get; }

    #endregion

    #region Public Methods

    Stream? LoadGameIcon();
    Task Initialize( IEditorEnvironment environment );

    #endregion

  }

}
