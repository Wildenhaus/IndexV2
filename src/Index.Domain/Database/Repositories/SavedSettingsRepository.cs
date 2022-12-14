using Index.Domain.Database.Entities;
using Index.Domain.Models;

namespace Index.Domain.Database.Repositories
{

  public class SavedSettingsRepository : GenericRepository<SavedSettings>, ISavedSettingsRepository
  {

    private readonly string _gameId;

    public SavedSettingsRepository( IndexDataContext context, IEditorEnvironment environment )
      : base( context )
    {
      _gameId = environment.GameId;
    }

    public SavedSettings GetByKey( string key )
      => Set.Where( x => x.GameId == _gameId && x.Key == key ).FirstOrDefault();

    public void AddOrUpdate( SavedSettings settings )
    {
      if ( settings.Id == 0 )
        Set.Add( settings );
      else
        Set.Update( settings );
    }

    public SavedSettings New( string key )
    {
      var settings = new SavedSettings();
      settings.GameId = _gameId;
      settings.Key = key;
      settings.Data = string.Empty;

      Add( settings );
      SaveChanges();

      return settings;
    }

  }

}
