using System.Reflection;
using System.Runtime.Loader;

namespace Index.Domain.GameProfiles
{

  public class GameProfileManager : IGameProfileManager
  {

    #region Constants

    private const string PROFILE_DIRECTORY_NAME = "GameProfiles";

    #endregion

    #region Data Members

    private Dictionary<string, IGameProfile> _profiles;

    #endregion

    #region Properties

    public IReadOnlyDictionary<string, IGameProfile> Profiles
    {
      get => _profiles;
    }

    #endregion

    #region Constructor

    public GameProfileManager()
    {
      _profiles = new Dictionary<string, IGameProfile>();

      LoadGameProfiles();
    }

    #endregion

    #region Public Methods

    public IList<IdentifiedGamePath> ScanPathForSupportedGames( string path )
    {
      var pathIdentifier = new GamePathIdentifier( this );
      return pathIdentifier.IdentifyCompatableProfiles( path ).ToList();
    }

    #endregion

    #region Private Methods

    private void LoadGameProfiles()
    {
      var profilePath = ResolveProfilePath();
      if ( !Directory.Exists( profilePath ) )
        return;

      foreach ( var assemblyPath in GetGameProfileAssemblyPaths() )
      {
        var assembly = Assembly.LoadFrom( assemblyPath );
        foreach ( var profileType in EnumerateGameProfileTypes( assembly ) )
        {
          var instance = ( IGameProfile ) Activator.CreateInstance( profileType );
          _profiles.Add( instance.GameId, instance );
        }
      }
    }

    private IEnumerable<string> GetGameProfileAssemblyPaths()
    {
      var profilePath = ResolveProfilePath();

      // Load Assemblies
      var preloadContext = new AssemblyLoadContext( "GameProfilePreloadContext", true );
      var assemblyPaths = Directory.GetFiles( profilePath, "*.dll" );
      foreach ( var assemblyPath in assemblyPaths )
      {
        var assembly = preloadContext.LoadFromAssemblyPath( assemblyPath );

        if ( EnumerateGameProfileTypes( assembly ).Any() )
          yield return assemblyPath;
      }

      preloadContext.Unload();
    }

    private static string ResolveProfilePath()
    {
      var appPath = Path.GetDirectoryName( Environment.ProcessPath );
      return Path.Combine( appPath, PROFILE_DIRECTORY_NAME );
    }

    private static IEnumerable<Type> EnumerateGameProfileTypes( Assembly assembly )
    {
      return assembly.GetTypes()
          .Where( x => x.IsAssignableTo( typeof( IGameProfile ) ) )
          .Where( x => !x.IsAbstract );
    }

    #endregion

  }

}
