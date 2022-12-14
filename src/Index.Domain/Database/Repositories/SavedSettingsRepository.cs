using Index.Domain.Database.Entities;
using Index.Domain.Models;

namespace Index.Domain.Database.Repositories
{

  public class SavedSettingsRepository : GenericRepository<SavedSettings>, ISavedSettingsRepository
  {

    private readonly string _gameId;

    protected SavedSettingsRepository( IndexDataContext context, IEditorEnvironment environment )
      : base( context )
    {
      _gameId = environment.GameId;
    }

    public SavedSettings GetByKey( string key )
      => Set.Where( x => x.GameId == _gameId && x.Key == key ).FirstOrDefault();

  }

}
