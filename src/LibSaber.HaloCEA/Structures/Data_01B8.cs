using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.SceneProps )]
  public class SaberPropsList : List<Data_01B8_Entry>
  {

    #region Constructor

    public SaberPropsList()
    {
    }

    public SaberPropsList( int capacity )
      : base( capacity )
    {
    }

    #endregion

    #region Serialization

    public static SaberPropsList Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadInt32();
      var data = new SaberPropsList( count );

      for ( var i = 0; i < count; i++ )
        data.Add( Data_01B8_Entry.Deserialize( reader, context ) );

      return data;
    }

    #endregion

  }

  public class Data_01B8_Entry
  {

    #region Data Members

    public Data_01B9 PropInfo;
    public string Affixes;

    #endregion

    #region Serialization

    public static Data_01B8_Entry Deserialize( NativeReader reader, ISerializationContext context )
    {
      var data = new Data_01B8_Entry();

      var sentinelReader = new SentinelReader( reader );

      sentinelReader.Next();
      ASSERT( sentinelReader.SentinelId == SentinelIds.Sentinel_01B9 );
      data.PropInfo = Data_01B9.Deserialize( reader, context );

      sentinelReader.Next();
      ASSERT( sentinelReader.SentinelId == SentinelIds.Sentinel_01BB );
      data.Affixes = reader.ReadNullTerminatedString();

      return data;
    }

    #endregion

  }

}
