using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0135 )]
  public class Data_0135
  {

    #region Data Members

    public Vector3<short> Translation;
    public Vector3<short> Scale;

    #endregion

    #region Serialization

    public static Data_0135 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_0135();

      data.Translation = Vector3<short>.Deserialize( reader, context );
      data.Scale = Vector3<short>.Deserialize( reader, context );

      return data;
    }

    #endregion

  }

}
