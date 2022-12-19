using System.Numerics;
using LibSaber.Halo2A.Structures.Materials;

namespace LibSaber.Halo2A.Structures
{

  public class GeometrySubMesh
  {

    #region Properties

    public GeometrySubMeshBufferInfo BufferInfo { get; set; }
    public int MeshId { get; set; } // TODO: This is a guess

    public Vector3? Position { get; set; }
    public Vector3? Scale { get; set; }

    public ushort NodeId { get; set; }
    public SaberMaterial Material { get; set; }

    public short[] BoneIds { get; set; }
    public Dictionary<byte, short> UvScaling { get; set; }

    #endregion

    #region

    public GeometrySubMesh()
    {
      UvScaling = new Dictionary<byte, short>();
    }

    #endregion

  }

}
