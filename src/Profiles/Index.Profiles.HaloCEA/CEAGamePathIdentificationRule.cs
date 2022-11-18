using Index.Domain.GameProfiles;

namespace Index.Profiles.HaloCEA
{

  public class CEAGamePathIdentificationRule : IGamePathIdentificationRule
  {

    public string BaseDirectoryName => "halo1";

    public IEnumerable<string> ChildPaths
    {
      get
      {
        yield return @"halo1.dll";
        yield return @"prebuild\paks\";
      }
    }

    public IEnumerable<string> ParentPaths => Enumerable.Empty<string>();

  }

}
