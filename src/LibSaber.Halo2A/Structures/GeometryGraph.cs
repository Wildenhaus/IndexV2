namespace LibSaber.Halo2A.Structures
{

  public class GeometryGraph
  {

    #region Properties

    public List<SaberObject> Objects { get; set; }
    public string[] ObjectProps { get; set; }
    public List<ObjectLodRoot> LodRoots { get; set; }

    public short RootNodeIndex { get; set; }
    public int NodeCount { get; set; }
    public int BufferCount { get; set; }
    public int MeshCount { get; set; }
    public int SubMeshCount { get; set; }

    public List<GeometryBuffer> Buffers { get; set; }
    public List<GeometryMesh> Meshes { get; set; }
    public List<GeometrySubMesh> SubMeshes { get; set; }

    public SaberObject RootObject
    {
      get => Objects[ RootNodeIndex ];
    }

    #endregion

  }

}