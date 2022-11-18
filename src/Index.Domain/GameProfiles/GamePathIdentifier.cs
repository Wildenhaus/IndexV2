namespace Index.Domain.GameProfiles
{

  public class GamePathIdentifier
  {

    private readonly IGameProfileManager _profileManager;
    private readonly Dictionary<string, IGamePathIdentificationRule> _rules;

    public GamePathIdentifier( IGameProfileManager profileManager )
    {
      _profileManager = profileManager;

      var profiles = _profileManager.Profiles.Values;
      _rules = profiles.ToDictionary( x => x.GameId, x => x.IdentificationRule );
    }

    public IEnumerable<IdentifiedGamePath> IdentifyCompatableProfiles( string pathToIdentify )
    {
      var targetPaths = Directory
        .EnumerateDirectories( pathToIdentify, "*", SearchOption.AllDirectories )
        .Append( pathToIdentify );

      foreach ( (string gameId, IGamePathIdentificationRule rule) in _rules )
      {
        if ( !TryIdentifyPathWithRule( rule, targetPaths, out var foundPath ) )
          continue;

        yield return new IdentifiedGamePath
        {
          GameId = gameId,
          GamePath = foundPath
        };
      }
    }

    private bool TryIdentifyPathWithRule( IGamePathIdentificationRule rule, IEnumerable<string> targetPaths, out string foundPath )
    {
      foundPath = default;

      var baseDirectory = rule.BaseDirectoryName;
      foreach ( var targetPath in targetPaths )
      {
        // Check if it matches the rule's BaseDirectoryName
        var targetPathName = Path.GetFileNameWithoutExtension( targetPath );
        if ( !targetPathName.Equals( baseDirectory, StringComparison.InvariantCultureIgnoreCase ) )
          continue;

        if ( PathMatchesAllChildPathRules( targetPath, rule.ChildPaths ) )
        {
          foundPath = targetPath;
          return true;
        }
      }

      return false;
    }

    private bool PathMatchesAllChildPathRules( string targetPath, IEnumerable<string> childPaths )
    {
      foreach ( var childPath in childPaths )
      {
        var expectedPath = Path.Combine( targetPath, childPath );
        if ( !File.Exists( expectedPath ) && !Directory.Exists( expectedPath ) )
          return false;
      }

      return true;
    }
  }

  public struct IdentifiedGamePath
  {
    public string GameId;
    public string GamePath;
  }

}
