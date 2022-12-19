using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class LodDefinitionSerializer : H2ASerializerBase<List<LodDefinition>>
  {

    #region Overrides

    public override List<LodDefinition> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadInt32();
      var propertyCount = reader.ReadInt32();

      var lodDefs = new List<LodDefinition>( count );
      for ( var i = 0; i < count; i++ )
        lodDefs.Add( new LodDefinition() );

      ReadObjectIdProperty( reader, lodDefs );
      ReadIndexProperty( reader, lodDefs );
      ReadIsLastLodProperty( reader, lodDefs );

      return lodDefs;
    }

    #endregion

    #region Private Methods

    private void ReadObjectIdProperty( NativeReader reader, List<LodDefinition> lodDefs )
    {
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var lodDef in lodDefs )
        lodDef.ObjectId = reader.ReadInt16();
    }

    private void ReadIndexProperty( NativeReader reader, List<LodDefinition> lodDefs )
    {
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var lodDef in lodDefs )
        lodDef.Index = reader.ReadByte();
    }

    private void ReadIsLastLodProperty( NativeReader reader, List<LodDefinition> lodDefs )
    {
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var lodDef in lodDefs )
        lodDef.IsLastLodUpToInfinity = reader.ReadBoolean();
    }

    #endregion

  }

}
