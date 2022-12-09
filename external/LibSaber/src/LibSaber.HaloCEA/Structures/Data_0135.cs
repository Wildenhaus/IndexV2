using System.Numerics;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0135 )]
  public class Data_0135
  {

    #region Data Members

    public Vector3 Translation;
    public Vector3 Scale;

    #endregion

    #region Serialization

    public static Data_0135 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0135();

      data.Translation = new Vector3( reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16() );
      data.Scale = new Vector3( reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16() );

      return data;
    }

    #endregion

  }

}
