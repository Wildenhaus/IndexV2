using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_01EA )]
  public class Data_01EA : List<Data_02E4>
  {

    #region Constructor

    public Data_01EA()
    {
    }

    public Data_01EA( int capacity )
      : base( capacity )
    {
    }

    #endregion

    #region Serialization

    public static Data_01EA Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadInt32();

      var data = new Data_01EA();

      var sentinelReader = new SentinelReader( reader );
      for ( var i = 0; i < count; i++ )
      {
        sentinelReader.Next();
        data.Add( Data_02E4.Deserialize( reader, context ) );
      }

      return data;
    }

    #endregion

  }

}
