namespace LibSaber.Halo2A.Structures
{

  public class ObjectLodRoot
  {

    #region Properties

    public List<uint> ObjectIds { get; set; }
    public List<uint> MaxObjectLodIndices { get; set; }
    public List<LodDistance> LodDistances { get; set; }
    public Box BoundingBox { get; set; }

    #endregion

  }

}
