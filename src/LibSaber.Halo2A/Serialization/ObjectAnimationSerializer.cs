using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class ObjectAnimationSerializer : H2ASerializerBase<List<ObjectAnimation>>
  {

    #region Overrides

    public override List<ObjectAnimation> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadInt32();
      var propertyCount = reader.ReadUInt32();

      var animList = new List<ObjectAnimation>( count );
      for ( var i = 0; i < count; i++ )
        animList.Add( new ObjectAnimation() );

      if ( propertyCount > 0 )
        ReadIniTranslationProperty( reader, animList );
      if ( propertyCount > 1 )
        ReadPTranslationProperty( reader, animList, context );
      if ( propertyCount > 2 )
        ReadIniRotationProperty( reader, animList );
      if ( propertyCount > 3 )
        ReadPRotationProperty( reader, animList, context );
      if ( propertyCount > 4 )
        ReadIniScaleProperty( reader, animList );
      if ( propertyCount > 5 )
        ReadPScaleProperty( reader, animList, context );
      if ( propertyCount > 6 )
        ReadIniVisibilityProperty( reader, animList );
      if ( propertyCount > 7 )
        ReadPVisibilityProperty( reader, animList, context );

      return animList;
    }

    #endregion

    #region Private Methods

    private void ReadIniTranslationProperty( NativeReader reader, List<ObjectAnimation> animList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var anim in animList )
        anim.IniTranslation = reader.ReadVector3();
    }

    private void ReadPTranslationProperty( NativeReader reader, List<ObjectAnimation> animList, ISerializationContext context )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      var propertyFlags = reader.ReadBitArray( animList.Count );

      var serializer = new SplineSerializer();
      for ( var i = 0; i < animList.Count; i++ )
        if ( propertyFlags[ i ] )
          animList[ i ].PTranslation = serializer.Deserialize( reader, context );
    }

    private void ReadIniRotationProperty( NativeReader reader, List<ObjectAnimation> animList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var anim in animList )
        anim.IniRotation = reader.ReadVector4();
    }

    private void ReadPRotationProperty( NativeReader reader, List<ObjectAnimation> animList, ISerializationContext context )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      var propertyFlags = reader.ReadBitArray( animList.Count );

      var serializer = new SplineSerializer();
      for ( var i = 0; i < animList.Count; i++ )
        if ( propertyFlags[ i ] )
          animList[ i ].PRotation = serializer.Deserialize( reader, context );
    }

    private void ReadIniScaleProperty( NativeReader reader, List<ObjectAnimation> animList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var anim in animList )
        anim.IniScale = reader.ReadVector3();
    }

    private void ReadPScaleProperty( NativeReader reader, List<ObjectAnimation> animList, ISerializationContext context )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      var propertyFlags = reader.ReadBitArray( animList.Count );

      var serializer = new SplineSerializer();
      for ( var i = 0; i < animList.Count; i++ )
        if ( propertyFlags[ i ] )
          animList[ i ].PScale = serializer.Deserialize( reader, context );
    }

    private void ReadIniVisibilityProperty( NativeReader reader, List<ObjectAnimation> animList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var anim in animList )
        anim.IniVisibility = reader.ReadFloat32();
    }

    private void ReadPVisibilityProperty( NativeReader reader, List<ObjectAnimation> animList, ISerializationContext context )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      var propertyFlags = reader.ReadBitArray( animList.Count );

      var serializer = new SplineSerializer();
      for ( var i = 0; i < animList.Count; i++ )
        if ( propertyFlags[ i ] )
          animList[ i ].PVisibility = serializer.Deserialize( reader, context );
    }

    #endregion

  }

}
