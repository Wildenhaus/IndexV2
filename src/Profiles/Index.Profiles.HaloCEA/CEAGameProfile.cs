using Index.Domain.FileSystem;
using Index.Domain.GameProfiles;
using Index.Profiles.HaloCEA.FileSystem;

namespace Index.Profiles.HaloCEA
{

  public class CEAGameProfile : GameProfileBase
  {

    #region Properties

    public override string GameId => "HaloCEA";
    public override string GameName => "Halo: Combat Evolved Anniversary";
    public override string Author => "Haus";

    public override IFileSystemLoader FileSystemLoader => new CEAFileSystemLoader();
    public override IGamePathIdentificationRule IdentificationRule => new CEAGamePathIdentificationRule();

    #endregion

  }

}
