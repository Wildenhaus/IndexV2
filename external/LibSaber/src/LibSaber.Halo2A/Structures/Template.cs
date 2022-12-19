namespace LibSaber.Halo2A.Structures
{

  public class Template
  {

    #region Properties

    public string Name { get; set; }
    public string NameClass { get; set; }
    public string Affixes { get; set; }
    public string PS { get; set; }
    // Skin
    public AnimationTrack AnimTrack { get; set; }
    public Box BoundingBox { get; set; }
    public List<LodDefinition> LodDefinitions { get; set; }
    public List<string> TexList { get; set; }
    public GeometryGraph GeometryGraph { get; set; }

    #endregion

  }

}
