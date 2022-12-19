using Index.Domain.GameProfiles;

namespace Index.Profiles.Halo2A
{

  public class H2AGamePathIdentificationRule : IGamePathIdentificationRule
  {

    public string BaseDirectoryName => "halo2";

    public IEnumerable<string> ChildPaths
    {
      get
      {
        yield return @"halo2.dll";
        yield return @"preload\paks\";
      }
    }

  }

}
