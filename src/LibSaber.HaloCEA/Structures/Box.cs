using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.HaloCEA.Structures
{

  public readonly struct Box
  {

    #region Data Members

    public readonly int Unk;
    public readonly Vector3 Min;
    public readonly Vector3 Max;

    #endregion

    #region Constructor

    public Box( Vector3 min, Vector3 max )
    {
      Unk = 0;
      Min = min;
      Max = max;
    }

    public Box( int unk, Vector3 min, Vector3 max )
    {
      Unk = unk;
      Min = min;
      Max = max;
    }

    #endregion

    #region Serialization

    public static Box Deserialize( NativeReader reader, ISerializationContext context )
    {
      var unk = reader.ReadInt32();
      var min = reader.ReadVector3();
      var max = reader.ReadVector3();

      return new Box( unk, min, max );
    }

    #endregion

  }

}
