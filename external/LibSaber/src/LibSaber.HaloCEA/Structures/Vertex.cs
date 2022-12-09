using System.Numerics;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  public struct Vertex
  {

    #region Data Members

    public Vector3 Position;
    public Vector3 Normal;

    #endregion

    #region Constructor

    public Vertex( Vector3 position, Vector3 normal )
    {
      Position = position;
      Normal = normal;
    }

    public Vertex( Vector3<SNorm16> position, Vector3<SNorm16> normal )
    {
      Position = new Vector3( position.X, position.Y, position.Z );
      Normal = new Vector3( normal.X, normal.Y, normal.Z );
    }

    #endregion

  }

}
