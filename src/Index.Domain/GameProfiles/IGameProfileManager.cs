namespace Index.Domain.GameProfiles
{

  public interface IGameProfileManager
  {

    IReadOnlyDictionary<string, IGameProfile> Profiles { get; }

    IList<IdentifiedGamePath> ScanPathForSupportedGames( string path );

  }

}
