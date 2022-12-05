using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_01BA )]
  [SaberInternalName( "psENTRY" )]
  public class Data_01BA : TypeWrapper<string>
  {

    public static Data_01BA Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_01BA();

      data.Value = reader.ReadNullTerminatedString();

      return data;
    }

  }

}
