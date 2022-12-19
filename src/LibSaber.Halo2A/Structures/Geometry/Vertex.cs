using System.Numerics;

namespace LibSaber.Halo2A.Structures.Geometry
{

  public struct Vertex
  {

    public Vector4 Position;
    public Vector4? Normal;

    public byte Index1 { get; set; }
    public byte Index2 { get; set; }
    public byte Index3 { get; set; }
    public byte Index4 { get; set; }

    public float? Weight1 { get; set; }
    public float? Weight2 { get; set; }
    public float? Weight3 { get; set; }
    public float? Weight4 { get; set; }

    public bool HasSkinningData
    {
      get => Weight1.HasValue;
    }

  }

}
