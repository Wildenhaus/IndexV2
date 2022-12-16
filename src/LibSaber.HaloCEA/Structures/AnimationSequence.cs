using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.AnimationSequence )]
  [SaberInternalName( "animSEQ" )]
  public struct AnimationSequence
  {

    #region Data Members

    [Sentinel( SentinelIds.AnimationSequence_Name )]
    [SaberInternalName( "name" )]
    public string Name;

    [Sentinel( SentinelIds.AnimationSequence_TimeSec )]
    [SaberInternalName( "timeSec" )]
    public float UnkTimeSec;

    [Sentinel( SentinelIds.AnimationSequence_LenFrame )]
    [SaberInternalName( "lenFrame" )]
    public float LenFrame;

    [Sentinel( SentinelIds.AnimationSequence_StartFrame )]
    [SaberInternalName( "startFrame" )]
    public float StartFrame;

    [Sentinel( SentinelIds.AnimationSequence_EndFrame )]
    [SaberInternalName( "endFrame" )]
    public float EndFrame;

    [Sentinel( SentinelIds.AnimationSequence_OffsetFrame )]
    [SaberInternalName( "offsetFrame" )]
    public float OffsetFrame;

    [Sentinel( SentinelIds.AnimationSequence_BoundingBox )]
    [SaberInternalName( "bbox" )]
    public Box BoundingBox;

    [Sentinel( SentinelIds.AnimationSequence_ActionFrames )]
    public List<ActionFrame> ActionFrames;

    #endregion

    #region Serialization

    public static AnimationSequence Deserialize( NativeReader reader, ISerializationContext context )
    {
      var animSeq = new AnimationSequence();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.AnimationSequence_Name:
            animSeq.Name = reader.ReadNullTerminatedString();
            break;
          case SentinelIds.AnimationSequence_TimeSec:
            animSeq.UnkTimeSec = reader.ReadFloat32();
            break;
          case SentinelIds.AnimationSequence_LenFrame:
            animSeq.LenFrame = reader.ReadFloat32();
            break;
          case SentinelIds.AnimationSequence_StartFrame:
            animSeq.StartFrame = reader.ReadFloat32();
            break;
          case SentinelIds.AnimationSequence_EndFrame:
            animSeq.EndFrame = reader.ReadFloat32();
            break;
          case SentinelIds.AnimationSequence_OffsetFrame:
            animSeq.OffsetFrame = reader.ReadFloat32();
            break;
          case SentinelIds.AnimationSequence_BoundingBox:
            animSeq.BoundingBox = Box.Deserialize( reader, context );
            break;
          case SentinelIds.AnimationSequence_ActionFrames:
            animSeq.ActionFrames = DataList<ActionFrame>.Deserialize( reader, context, ActionFrame.Deserialize );
            break;

          case SentinelIds.Delimiter:
            return animSeq;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return animSeq;
    }

    #endregion

  }

}
