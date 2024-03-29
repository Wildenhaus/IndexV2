﻿using Index.Domain.Database.Entities;

namespace Index.Domain.Database.Repositories
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
