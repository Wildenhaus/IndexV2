using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class SaberObjectSerializer : H2ASerializerBase<List<SaberObject>>
  {

    #region Overrides

    public override List<SaberObject> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var graph = context.GetMostRecentObject<GeometryGraph>();
      var objectCount = reader.ReadInt16();

      var unk_01 = reader.ReadUInt16();
      var propertyCount = reader.ReadUInt16();
      var unk_03 = reader.ReadUInt16();

      var objects = new List<SaberObject>( objectCount );
      for ( var i = 0; i < objectCount; i++ )
        objects.Add( new SaberObject( graph ) );

      if ( propertyCount > 0 )
        ReadIdProperty( reader, objects );
      if ( propertyCount > 1 )
        ReadReadNameProperty( reader, objects );
      if ( propertyCount > 2 )
        ReadStateProperty( reader, objects );
      if ( propertyCount > 3 )
        ReadParentIdProperty( reader, objects );
      if ( propertyCount > 4 )
        ReadNextIdProperty( reader, objects );
      if ( propertyCount > 5 )
        ReadPrevIdProperty( reader, objects );
      if ( propertyCount > 6 )
        ReadChildIdProperty( reader, objects );
      if ( propertyCount > 7 )
        ReadAnimNumberProperty( reader, objects );
      if ( propertyCount > 8 )
        ReadReadAffixesProperty( reader, objects );
      if ( propertyCount > 9 )
        ReadMatrixLTProperty( reader, objects );
      if ( propertyCount > 10 )
        ReadMatrixModelProperty( reader, objects );
      if ( propertyCount > 11 )
        ReadGeomDataProperty( reader, objects );
      if ( propertyCount > 12 )
        ReadUnkNamesProperty( reader, objects );
      if ( propertyCount > 13 )
        ReadObbProperty( reader, objects );
      if ( propertyCount > 14 )
        ReadNameProperty( reader, objects );
      if ( propertyCount > 15 )
        ReadAffixesProperty( reader, objects );

      return objects;
    }

    #endregion

    #region Private Methods

    private void ReadIdProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.Id = reader.ReadInt16();
    }

    private void ReadReadNameProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.ReadName = reader.ReadLengthPrefixedString32();
    }

    private void ReadStateProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      for ( var i = 0; i < objects.Count; i++ )
      {
        _ = reader.ReadUInt16(); // TODO: Unk
        _ = reader.ReadUInt16(); // TODO: Unk
        _ = reader.ReadUInt16(); // TODO: Unk
        _ = reader.ReadByte();   // TODO: Unk
      }
    }

    private void ReadParentIdProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.ParentId = reader.ReadInt16();
    }

    private void ReadNextIdProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.NextId = reader.ReadInt16();
    }

    private void ReadPrevIdProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.PrevId = reader.ReadInt16();
    }

    private void ReadChildIdProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.ChildId = reader.ReadInt16();
    }

    private void ReadAnimNumberProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.AnimNumber = reader.ReadInt16();
    }

    private void ReadReadAffixesProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.ReadAffixes = reader.ReadLengthPrefixedString32();
    }

    private void ReadMatrixLTProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.MatrixLT = reader.ReadMatrix4x4();
    }

    private void ReadMatrixModelProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.MatrixModel = reader.ReadMatrix4x4();
    }

    private void ReadGeomDataProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      var objFlags = reader.ReadBitArray( objects.Count );

      for ( var i = 0; i < objects.Count; i++ )
      {
        if ( objFlags[ i ] )
        {
          var unk_01 = reader.ReadUInt32(); // TODO: 0x00000003
          var unk_02 = reader.ReadByte(); // TODO: 0x7
          ASSERT( unk_01 == 0x3, "3 val not found" );
          ASSERT( unk_02 == 0x7, "7 val not found" );

          var geomData = new ObjectGeometryUnshared();
          geomData.SplitIndex = reader.ReadUInt32();
          geomData.NumSplits = reader.ReadUInt32();
          geomData.BoundingBox = new Box( reader.ReadVector3(), reader.ReadVector3() );

          objects[ i ].GeomData = geomData;
        }
      }

      ASSERT( reader.PeekByte() != 0x3, "Still more objGEOM_UNSHARED data" );
    }

    private void ReadUnkNamesProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.UnkName = reader.ReadLengthPrefixedString32();
    }

    private void ReadObbProperty( NativeReader reader, List<SaberObject> objects )
    {
      // TODO: Move this into M3DOBB serializer/data class
      // This seems to be all zeroes.
      // Maybe this is the skip[dsString] property?

      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        for ( var j = 0; j < 60; j++ )
          reader.ReadByte();
    }

    private void ReadNameProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.Name = reader.ReadLengthPrefixedString16();
    }

    private void ReadAffixesProperty( NativeReader reader, List<SaberObject> objects )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var obj in objects )
        obj.Affixes = reader.ReadLengthPrefixedString16();
    }

    #endregion

  }

}
