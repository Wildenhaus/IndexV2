namespace Index.Domain.GameProfiles
{

  public interface IGamePathIdentificationRule
  {

    public string BaseDirectoryName { get; }
    public IEnumerable<string> ChildPaths { get; }

  }

}
