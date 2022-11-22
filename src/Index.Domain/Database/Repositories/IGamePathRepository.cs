using Index.Domain.Entities;

namespace Index.Domain.Database.Repositories
{

    public interface IGamePathRepository : IRepository<GamePath>
    {

        GamePath? GetByPath(string gamePath);
        bool CheckIfGamePathAlreadyExists(string gamePath);

    }

}
