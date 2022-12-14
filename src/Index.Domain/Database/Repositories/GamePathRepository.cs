using Index.Domain.Database.Entities;

namespace Index.Domain.Database.Repositories
{

  public sealed class GamePathRepository : GenericRepository<GamePath>, IGamePathRepository
  {

    public GamePathRepository( IndexDataContext context )
      : base( context )
    {
    }

    public GamePath? GetByPath( string path )
      => Set.FirstOrDefault( x => x.Path == path );

    public bool CheckIfGamePathAlreadyExists( string gamePath )
      => Set.Any( x => x.Path == gamePath );

  }

}
