using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_0482 )]
  [SaberInternalName( "psSECTION" )]
  public class Data_0482 : List<Data_01BA>
  {

    #region Constructor

    public Data_0482()
    {
    }

    public Data_0482( int capacity )
      : base( capacity )
    {
    }

    #endregion

    #region Serialization

    public static Data_0482 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadInt32();
      var data = new Data_0482( count );

      var sentinelReader = new SentinelReader( reader );
      for ( var i = 0; i < count; i++ )
      {
        sentinelReader.Next();
        data.Add( Data_01BA.Deserialize( reader, context ) );
      }

      return data;
    }

    #endregion

  }

}
