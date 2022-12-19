using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class AnimationSequenceSerializer : H2ASerializerBase<List<AnimationSequence>>
  {

    #region Overrides

    public override List<AnimationSequence> Deserialize( NativeReader reader, ISerializationContext context )
    {

      var count = reader.ReadInt32();
      var propertyCount = reader.ReadInt32();

      var seqList = new List<AnimationSequence>( count );
      for ( var i = 0; i < count; i++ )
        seqList.Add( new AnimationSequence() );

      ReadNameProperty( reader, seqList );
      ReadLayerIdProperty( reader, seqList );
      ReadStartFrameProperty( reader, seqList );
      ReadEndFrameProperty( reader, seqList );
      ReadOffsetFrameProperty( reader, seqList );
      ReadLenFrameProperty( reader, seqList );
      ReadTimeSecProperty( reader, seqList );
      ReadActionFramesProperty( reader, seqList );
      ReadBoundingBoxProperty( reader, seqList );

      return seqList;
    }

    #endregion

    #region Private Methods

    private void ReadNameProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].Name = reader.ReadLengthPrefixedString32();
    }

    private void ReadLayerIdProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].LayerId = reader.ReadUInt32();
    }

    private void ReadStartFrameProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].StartFrame = reader.ReadFloat32();
    }

    private void ReadEndFrameProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].EndFrame = reader.ReadFloat32();
    }

    private void ReadOffsetFrameProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].OffsetFrame = reader.ReadFloat32();
    }

    private void ReadLenFrameProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].LenFrame = reader.ReadFloat32();
    }

    private void ReadTimeSecProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].TimeSec = reader.ReadFloat32();
    }

    private void ReadActionFramesProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      var serializer = Serializer.GetSerializer<List<ActionFrame>>();
      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].ActionFrames = serializer.Deserialize( reader, null );
    }

    private void ReadBoundingBoxProperty( NativeReader reader, List<AnimationSequence> seqList )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < seqList.Count; i++ )
        seqList[ i ].BoundingBox = new Box( reader.ReadVector3(), reader.ReadVector3() );
    }

    #endregion

  }

}
