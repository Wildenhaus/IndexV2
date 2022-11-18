using Index.Domain.GameProfiles;

namespace Index.Profiles.HaloCEA
{

  public class CEAGameProfile : GameProfileBase
  {

    #region Properties

    public override string GameId => "HaloCEA";
    public override string GameName => "Halo: Combat Evolved Anniversary";
    public override string Author => "Haus";

    public override IGamePathIdentificationRule IdentificationRule
      => new CEAGamePathIdentificationRule();


    #endregion

  }

}
