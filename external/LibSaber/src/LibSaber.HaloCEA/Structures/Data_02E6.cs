using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_02E6 )]
  public struct Data_02E6
  {

    #region Data Members

    public int Count;
    public List<AnimationSequence> Entries;

    #endregion

    #region Casts

    public static implicit operator List<AnimationSequence>( Data_02E6 dataList )
      => dataList.Entries;

    #endregion

    #region Serialization

    public static Data_02E6 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_02E6();

      var count = data.Count = reader.ReadInt32();

      var sentinelReader = new SentinelReader( reader );
      var entries = data.Entries = new List<AnimationSequence>( count );
      for ( var i = 0; i < count; i++ )
      {
        sentinelReader.Next();
        entries.Add( AnimationSequence.Deserialize( reader, context ) );
      }

      return data;
    }

    #endregion

  }

}
