namespace Index.Domain.Models
{

  public interface IEditorEnvironment
  {

    public string GameId { get; set; }
    public string GameName { get; set; }
    public string GamePath { get; set; }

  }

}
