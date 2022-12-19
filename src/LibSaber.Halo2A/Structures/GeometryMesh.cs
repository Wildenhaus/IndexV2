using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Structures
{

  public class GeometryMesh
  {

    #region Properties

    public uint Id { get; set; }
    public BitSet<short> Flags { get; set; }
    public GeometryMeshBufferInfo[] Buffers { get; set; }

    #endregion

  }

}
