using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class ObjectLodRootSerializer : H2ASerializerBase<List<ObjectLodRoot>>
  {

    #region Overrides

    public override List<ObjectLodRoot> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadUInt32();
      var propertyCount = reader.ReadUInt32();

      var lodRoots = new List<ObjectLodRoot>();
      for ( var i = 0; i < count; i++ )
        lodRoots.Add( new ObjectLodRoot() );

      ReadObjectIdsProperty( reader, lodRoots );
      ReadMaxObjectLodIndicesProperty( reader, lodRoots );
      ReadLodDistancesProperty( reader, lodRoots, context );
      ReadBoundingBoxProperty( reader, lodRoots );

      return lodRoots;
    }

    #endregion

    #region Private Methods

    private void ReadObjectIdsProperty( NativeReader reader, List<ObjectLodRoot> lodRoots )
    {
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var lodRoot in lodRoots )
      {
        var count = reader.ReadInt32();
        lodRoot.ObjectIds = new List<uint>( count );
        for ( var i = 0; i < count; i++ )
          lodRoot.ObjectIds.Add( reader.ReadUInt32() );
      }
    }

    private void ReadMaxObjectLodIndicesProperty( NativeReader reader, List<ObjectLodRoot> lodRoots )
    {
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var lodRoot in lodRoots )
      {
        var count = reader.ReadInt32();
        lodRoot.MaxObjectLodIndices = new List<uint>( count );
        for ( var i = 0; i < count; i++ )
          lodRoot.MaxObjectLodIndices.Add( reader.ReadUInt32() );
      }
    }

    private void ReadLodDistancesProperty( NativeReader reader, List<ObjectLodRoot> lodRoots, ISerializationContext context )
    {
      if ( reader.ReadByte() == 0 )
        return;

      var serializer = new LodDistanceSerializer();
      foreach ( var lodRoot in lodRoots )
        lodRoot.LodDistances = serializer.Deserialize( reader, context );
    }

    private void ReadBoundingBoxProperty( NativeReader reader, List<ObjectLodRoot> lodRoots )
    {
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var lodRoot in lodRoots )
        lodRoot.BoundingBox = new Box( reader.ReadVector3(), reader.ReadVector3() );
    }

    #endregion

  }

}
