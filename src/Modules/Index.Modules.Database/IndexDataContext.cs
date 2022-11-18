using Index.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Index.Modules.Database
{

  public class IndexDataContext : DbContext
  {

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
      optionsBuilder.UseSqlite( "Data Source=index.db" );
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<GamePath>();
    }

  }

}
