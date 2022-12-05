using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_030E )]
  public struct Data_030E
  {

    #region Data Members

    public int Count;
    public List<Data_030F> Entries;

    #endregion

    public static implicit operator List<Data_030F>( Data_030E list )
      => list.Entries;

    #region Serialization

    public static Data_030E Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_030E();

      var count = data.Count = reader.ReadInt32();

      var sentinelReader = new SentinelReader( reader );
      var entries = data.Entries = new List<Data_030F>( count );
      for ( var i = 0; i < count; i++ )
      {
        sentinelReader.Next();
        entries.Add( Data_030F.Deserialize( reader, context ) );
      }

      return data;
    }

    #endregion

  }

}
