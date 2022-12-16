using System.Numerics;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.ObjectAnim )]
  [SaberInternalName( "animOBJ_ANIM" )]
  public struct ObjectAnimation
  {

    #region Data Members

    public int Unk_00;

    [Sentinel( SentinelIds.ObjectAnim_IniTranslation )]
    [SaberInternalName( "iniTranslation" )]
    public Vector3? TranslationInitial;

    [Sentinel( SentinelIds.ObjectAnim_PTranslation )]
    [SaberInternalName( "pTranslation" )]
    public SplineData? TranslationSpline;

    [Sentinel( SentinelIds.ObjectAnim_IniRotation )]
    [SaberInternalName( "iniRotation" )]
    public Vector4? RotationInitial;

    [Sentinel( SentinelIds.ObjectAnim_PRotation )]
    [SaberInternalName( "pRotation" )]
    public SplineData? RotationSpline;

    [Sentinel( SentinelIds.ObjectAnim_IniScale )]
    [SaberInternalName( "iniScale" )]
    public Vector3? ScaleInitial;

    [Sentinel( SentinelIds.ObjectAnim_PScale )]
    [SaberInternalName( "pScale" )]
    public SplineData? ScaleSpline;

    [Sentinel( SentinelIds.ObjectAnim_IniVisibility )]
    [SaberInternalName( "iniVisibility" )]
    public float? VisibilityInitial;

    [Sentinel( SentinelIds.ObjectAnim_PVisibility )]
    [SaberInternalName( "pVisibility" )]
    public SplineData? VisibilitySpline;

    #endregion

    #region Serialization

    public static ObjectAnimation Deserialize( NativeReader reader, ISerializationContext context )
    {
      var objectAnim = new ObjectAnimation();

      objectAnim.Unk_00 = reader.ReadInt32();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.ObjectAnim_PTranslation:
            objectAnim.TranslationSpline = SplineData.Deserialize( reader, context );
            break;
          case SentinelIds.ObjectAnim_PRotation:
            objectAnim.RotationSpline = SplineData.Deserialize( reader, context );
            break;
          case SentinelIds.ObjectAnim_PScale:
            objectAnim.ScaleSpline = SplineData.Deserialize( reader, context );
            break;
          case SentinelIds.ObjectAnim_PVisibility:
            objectAnim.VisibilitySpline = SplineData.Deserialize( reader, context );
            break;

          case SentinelIds.ObjectAnim_IniTranslation:
            objectAnim.TranslationInitial = reader.ReadVector3();
            break;
          case SentinelIds.ObjectAnim_IniRotation:
            objectAnim.RotationInitial = reader.ReadVector4();
            break;
          case SentinelIds.ObjectAnim_IniScale:
            objectAnim.ScaleInitial = reader.ReadVector3();
            break;
          case SentinelIds.ObjectAnim_IniVisibility:
            objectAnim.VisibilityInitial = reader.ReadFloat32();
            break;

          case SentinelIds.Delimiter:
            return objectAnim;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return objectAnim;
    }

    #endregion

  }

}
