using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.Sentinel_02E4 )]
  public class Data_02E4
  {

    #region Data Members

    [Sentinel( SentinelIds.Sentinel_02E5 )]
    public Data_02E5? TemplateInfo;

    [Sentinel( SentinelIds.Sentinel_0316 )]
    public Data_0316? Sentinel_0316;

    [Sentinel( SentinelIds.TextureList )]
    public TextureList TextureList;

    [Sentinel( SentinelIds.Sentinel_01BA )]
    public string? Sentinel_01BA;

    [Sentinel( SentinelIds.Sentinel_02E8 )]
    public List<ObjectAnimation> ObjectAnimations;

    [Sentinel( SentinelIds.Sentinel_02E6 )]
    public List<AnimationSequence> AnimationSequences;

    [Sentinel( SentinelIds.Sentinel_021D )]
    public int? Sentinel_021D;

    [Sentinel( SentinelIds.Sentinel_0304 )]
    public string? Sentinel_0304;

    [Sentinel( SentinelIds.Sentinel_0305 )]
    public Data_0305? Sentinel_0305;

    [Sentinel( SentinelIds.Sentinel_0308 )]
    public Box? BoundingBox;

    [Sentinel( SentinelIds.Sentinel_0312 )]
    public string? Sentinel_0312;

    [Sentinel( SentinelIds.Sentinel_0313 )]
    public List<Data_0313> Sentinel_0313;

    [Sentinel( SentinelIds.Sentinel_030E )]
    public List<Data_030F> Sentinel_030E;

    [Sentinel( SentinelIds.Sentinel_0311 )]
    public List<LodDefinition> LodDefinitions;

    [Sentinel( CEATemplateSentinels.ObjectList )]
    public SaberObjectList Objects;

    [Sentinel( SentinelIds.Sentinel_02E8 )]
    public Data_02E8 Animations;

    [Sentinel( SentinelIds.Sentinel_01BC )]
    public int Data_01BC;

    #endregion

    #region Serialization

    public static Data_02E4 Deserialize( NativeReader reader, ISerializationContext context )
    {
      //AssertTemplateSentinel( reader );

      var template = new Data_02E4();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_02E5:
            template.TemplateInfo = Data_02E5.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0316:
            template.Sentinel_0316 = Data_0316.Deserialize( reader, context );
            break;
          case SentinelIds.TextureList:
            template.TextureList = TextureList.Deserialize( reader, context );
            sentinelReader.BurnSentinel();
            break;
          case SentinelIds.Sentinel_01BA:
            template.Sentinel_01BA = reader.ReadNullTerminatedString();
            break;
          case SentinelIds.Sentinel_01BC:
            template.Data_01BC = reader.ReadInt32();
            break;
          case SentinelIds.Sentinel_021D:
            template.Sentinel_021D = reader.ReadInt32();
            break;
          case SentinelIds.Sentinel_02E8:
            template.ObjectAnimations = Data_02E8.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_02E6:
            template.AnimationSequences = Data_02E6.Deserialize( reader, context );
            sentinelReader.BurnSentinel();
            break;
          case SentinelIds.Sentinel_0304:
            template.Sentinel_0304 = reader.ReadNullTerminatedString();
            break;
          case SentinelIds.Sentinel_0305:
            template.Sentinel_0305 = Data_0305.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0308:
            template.BoundingBox = Box.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_030E:
            template.Sentinel_030E = Data_030E.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0311:
            template.LodDefinitions = Data_0311.Deserialize( reader, context );
            break;
          case SentinelIds.Sentinel_0312:
            template.Sentinel_0312 = reader.ReadLengthPrefixedString32();
            break;
          case SentinelIds.Sentinel_0313:
            template.Sentinel_0313 = DataList<Data_0313>.Deserialize( reader, context, Data_0313.Deserialize );
            break;

          case CEATemplateSentinels.ObjectList:
            template.Objects = SaberObjectList.Deserialize( reader, context );
            sentinelReader.BurnSentinel();
            break;

          case SentinelIds.Delimiter:
            return template;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return template;
    }

    private static void AssertTemplateSentinel( NativeReader reader )
    {
      /* Assert that we're at the beginning of a Template file.
       * Templates start with Sentinel 02E4, which encapsulates the entire file.
       */
      var sentinelReader = new SentinelReader( reader );

      ASSERT( sentinelReader.Next(), "Failed to read Template outer sentinel." );

      ASSERT( sentinelReader.SentinelId == SentinelIds.Sentinel_02E4,
        "Unexpected starting sentinel of Template: {0:X4} (Expected {1:X4}).",
        sentinelReader.SentinelId,
        SentinelIds.Sentinel_02E4 );
    }

    #endregion

    #region Template Sentinels

    private static class CEATemplateSentinels
    {
      public const SentinelId ObjectList = 0x00F0;
    }

    #endregion

  }

}
