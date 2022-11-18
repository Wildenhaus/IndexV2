using Index.Domain.Entities;

namespace Index.Domain.Repositories
{

  public interface IRepository<TEntity> : IDisposable
    where TEntity : EntityBase
  {

    void Add( TEntity entity );
    void Delete( TEntity entity );
    TEntity? Get( long id );
    IList<TEntity> GetAll();
    void Update( TEntity entity );

    void SaveChanges();

  }

}
