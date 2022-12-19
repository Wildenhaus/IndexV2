using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class LodDistanceSerializer : H2ASerializerBase<List<LodDistance>>
  {

    #region Overrides

    public override List<LodDistance> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var count = reader.ReadInt32();
      var propertyCount = reader.ReadUInt32();

      var lodDists = new List<LodDistance>( count );
      for ( var i = 0; i < count; i++ )
        lodDists.Add( new LodDistance() );

      ReadMaxDistanceProperty( reader, lodDists );

      return lodDists;
    }

    #endregion

    #region Private Methods

    private void ReadMaxDistanceProperty( NativeReader reader, List<LodDistance> lodDists )
    {
      if ( reader.ReadByte() == 0 )
        return;

      foreach ( var lodDist in lodDists )
        lodDist.MaxDistance = reader.ReadFloat32();
    }

    #endregion

  }

}
