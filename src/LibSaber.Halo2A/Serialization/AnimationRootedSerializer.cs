using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class AnimationRootedSerializer : H2ASerializerBase<AnimationRooted>
  {

    #region Overrides

    public override AnimationRooted Deserialize( NativeReader reader, ISerializationContext context )
    {
      var anim = new AnimationRooted();
      var properties = BitSet<int>.Deserialize( reader, context );

      if ( properties.HasFlag( AnimationRootedProperty.IniTranslation ) )
        anim.IniTranslation = reader.ReadVector3();

      if ( properties.HasFlag( AnimationRootedProperty.PTranslation ) )
        anim.PTranslation = Serializer<Spline>.Deserialize( reader, context );

      if ( properties.HasFlag( AnimationRootedProperty.IniRotation ) )
        anim.IniRotation = reader.ReadVector4();

      if ( properties.HasFlag( AnimationRootedProperty.PRotation ) )
        anim.PRotation = Serializer<Spline>.Deserialize( reader, context );

      return anim;
    }

    #endregion

    #region Property Flags

    [Flags]
    private enum AnimationRootedProperty : byte
    {
      IniTranslation = 1 << 0,
      PTranslation = 1 << 1,
      IniRotation = 1 << 2,
      PRotation = 1 << 3
    }

    #endregion

  }

}
