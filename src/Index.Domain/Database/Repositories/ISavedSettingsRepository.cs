using Index.Domain.Database.Entities;

namespace Index.Domain.Database.Repositories
{

  public interface ISavedSettingsRepository : IRepository<SavedSettings>
  {

    SavedSettings GetByKey( string key );

  }

}
