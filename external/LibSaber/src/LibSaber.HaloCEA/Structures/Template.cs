using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;

namespace LibSaber.HaloCEA.Structures
{

  public class Template
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_02E4 )]
    public Data_02E4 Data_02E4;

    #endregion

    #region Serialization

    public static Template Deserialize( NativeReader reader, ISerializationContext context )
    {
      var template = new Template();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_02E4:
            template.Data_02E4 = Data_02E4.Deserialize( reader, context );
            break;

          case SentinelIds.Delimiter:
            break;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return template;
    }

    #endregion

  }

}
