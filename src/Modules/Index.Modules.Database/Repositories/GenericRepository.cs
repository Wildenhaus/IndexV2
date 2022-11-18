using Index.Domain.Entities;
using Index.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Index.Modules.Database.Repositories
{

  public class GenericRepository<T> : DisposableObject, IRepository<T>
    where T : EntityBase
  {

    #region Data Members

    private readonly IndexDataContext _context;
    private readonly DbSet<T> _set;

    #endregion

    #region Properties

    protected DbSet<T> Set => _set;

    #endregion

    #region Constructor

    protected GenericRepository( IndexDataContext context )
    {
      _context = context;
      _set = context.Set<T>();
    }

    #endregion

    #region Public Methods

    public void Add( T entity )
      => _set.Add( entity );

    public void Delete( T entity )
      => _set.Remove( entity );

    public T? Get( long id )
      => _set.Find( id );

    public IList<T> GetAll()
      => _set.ToList();

    public void Update( T entity )
      => _set.Update( entity );

    public void SaveChanges()
      => _context.SaveChanges();

    #endregion

    #region Overrides

    protected override void OnDisposing()
      => _context?.Dispose();

    #endregion

  }

}
