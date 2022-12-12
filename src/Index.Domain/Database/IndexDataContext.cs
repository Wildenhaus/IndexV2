using Index.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Index.Domain.Database
{

  public class IndexDataContext : DbContext
  {

    protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    {
      var launchPath = Path.GetDirectoryName( Environment.ProcessPath );
      var dbPath = $"Data Source={Path.Combine( launchPath, "index.db" )}";
      optionsBuilder.UseSqlite( dbPath );
    }

    protected override void OnModelCreating( ModelBuilder modelBuilder )
    {
      modelBuilder.Entity<GamePath>();
    }

  }

}
