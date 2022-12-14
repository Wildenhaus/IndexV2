using Microsoft.EntityFrameworkCore;

namespace Index.Domain.Database.Entities
{

  [Index( nameof( GameId ), nameof( Key ), IsUnique = true )]
  public class SavedSettings : EntityBase
  {

    public string GameId { get; set; }

    public string Key { get; set; }
    public string Data { get; set; }

  }

}
