using Index.Domain.Assets;
using Index.Domain.FileSystem;
using Index.Domain.GameProfiles;

namespace Index.Domain.Models
{

  public class EditorEnvironment : IEditorEnvironment
  {
    public string GameId { get; set; }
    public string GameName { get; set; }
    public string GamePath { get; set; }
    public IGameProfile GameProfile { get; set; }

    public IAssetManager AssetManager { get; }
    public IFileSystem FileSystem { get; }

    public IParameterCollection GlobalParameters { get; }

    public EditorEnvironment( IAssetManager assetManager, IFileSystem fileSystem )
    {
      AssetManager = assetManager;
      FileSystem = fileSystem;

      GlobalParameters = new ParameterCollection();
    }

  }

}
