using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  public struct Vertex
  {

    #region Data Members

    public Vector3<float> Position;
    public Vector3<float> Normal;

    #endregion

    #region Constructor

    public Vertex( Vector3<float> position, Vector3<float> normal )
    {
      Position = position;
      Normal = normal;
    }

    public Vertex( Vector3<SNorm16> position, Vector3<SNorm16> normal )
    {
      Position = new Vector3<float>( position.X, position.Y, position.Z );
      Normal = new Vector3<float>( normal.X, normal.Y, normal.Z );
    }

    #endregion

  }

}
