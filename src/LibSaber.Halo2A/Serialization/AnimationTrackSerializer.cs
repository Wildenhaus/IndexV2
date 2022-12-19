using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class AnimationTrackSerializer : H2ASerializerBase<AnimationTrack>
  {

    #region Overrides

    public override AnimationTrack Deserialize( NativeReader reader, ISerializationContext context )
    {
      var track = new AnimationTrack();
      var propertyFlags = BitSet<int>.Deserialize( reader, context );

      if ( propertyFlags.HasFlag( AnimationTrackProperties.AnimationSequences ) )
        track.SeqList = new AnimationSequenceSerializer().Deserialize( reader, context );

      if ( propertyFlags.HasFlag( AnimationTrackProperties.ObjectAnimations ) )
        track.ObjAnimList = new ObjectAnimationSerializer().Deserialize( reader, context );

      if ( propertyFlags.HasFlag( AnimationTrackProperties.ObjectMap ) )
        FAIL( "This property has never been observed as in-use, and as such is not implemented." );

      if ( propertyFlags.HasFlag( AnimationTrackProperties.RootAnimation ) )
        track.RootAnim = new AnimationRootedSerializer().Deserialize( reader, context );

      return track;
    }

    #endregion

    #region Property Flags

    private enum AnimationTrackProperties : byte
    {
      AnimationSequences = 1 << 0,
      ObjectAnimations = 1 << 1,
      ObjectMap = 1 << 2,
      RootAnimation = 1 << 3,
    }

    #endregion

  }

}
