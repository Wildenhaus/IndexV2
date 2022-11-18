namespace Index.Domain.GameProfiles
{

  public abstract class GameProfileBase : IGameProfile
  {

    #region Properties

    public abstract string GameId { get; }
    public abstract string GameName { get; }
    public abstract string Author { get; }
    public virtual Version Version => DefaultGetVersion();

    public abstract IGamePathIdentificationRule IdentificationRule { get; }

    #endregion

    #region Public Methods

    public virtual Stream? LoadGameIcon()
    {
      const string DEFAULT_GAME_ICON_FILENAME = "GameIcon";

      var assembly = GetType().Assembly;
      var assemblyName = assembly.GetName().Name;

      var gameIconResourceName = assembly.GetManifestResourceNames()
        .Where( x =>
        {
          var resourceFileName = x.Replace( $"{assemblyName}.", "" );
          var resourceName = Path.GetFileNameWithoutExtension( resourceFileName );
          return resourceName.Equals( DEFAULT_GAME_ICON_FILENAME, StringComparison.InvariantCultureIgnoreCase );
        } )
        .FirstOrDefault();

      if ( gameIconResourceName is null )
        return null;

      return assembly.GetManifestResourceStream( gameIconResourceName );
    }

    #endregion

    #region Private Methods

    private Version DefaultGetVersion()
    {
      var assembly = GetType().Assembly;
      var version = assembly.GetName().Version;
      return version ?? new Version( 0, 0 );
    }

    #endregion

  }

}
