using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class GeometryGraphSerializer : H2ASerializerBase<GeometryGraph>
  {

    #region Constants

    private const uint SIGNATURE_OGM1 = 0x314D474F; //OGM1

    #endregion

    #region Overrides

    public override GeometryGraph Deserialize( NativeReader reader, ISerializationContext context )
    {
      ReadSignature( reader, SIGNATURE_OGM1 );

      var graph = new GeometryGraph();
      context.AddObject( graph );

      // TODO: These are guesses.
      /* The "GraphType" seems to denote the presence of some of these properties.
       * ReadObjectPsProperty() is very hacky, and doesn't seem to correlate with
       * either of these types.
       */
      var graphType = ( GeometryGraphType ) reader.ReadUInt16();
      var unk_02 = reader.ReadUInt16(); // TODO
      var unk_03 = reader.ReadUInt16(); // TODO

      ReadObjectsProperty( reader, graph, context );

      if ( graphType == GeometryGraphType.Props )
        ReadObjectPropsProperty( reader, graph );


      if ( graphType == GeometryGraphType.Grass )
      {
        ReadObjectPsProperty( reader, graph ); // TODO: This is hacky.
        ReadLodRootsProperty( reader, graph, context );
      }

      ReadData( reader, graph, context );

      return graph;
    }

    #endregion

    #region Property Read Methods

    private void ReadObjectsProperty( NativeReader reader, GeometryGraph graph, ISerializationContext context )
    {
      // Read Sentinel
      if ( reader.ReadByte() == 0 )
        return;

      var objectSerializer = new SaberObjectSerializer();
      graph.Objects = objectSerializer.Deserialize( reader, context );
    }

    private void ReadObjectPropsProperty( NativeReader reader, GeometryGraph graph )
    {
      // This section only seems to be present in ss_prop__h.tpl
      var count = reader.ReadByte();

      var unk_01 = reader.ReadByte(); // TODO
      var unk_02 = reader.ReadByte(); // TODO
      var unk_03 = reader.ReadByte(); // TODO
      var unk_04 = reader.ReadByte(); // TODO

      var props = graph.ObjectProps = new string[ count ];

      for ( var i = 0; i < count; i++ )
      {
        var unk_05 = reader.ReadUInt32(); // TODO
        props[ i ] = reader.ReadLengthPrefixedString32();
      }
    }

    private void ReadObjectPsProperty( NativeReader reader, GeometryGraph graph )
    {
      var count = reader.ReadInt32();
      var unk_01 = reader.ReadByte(); // TODO

      // TODO: This is a hack.
      // Not yet sure how to tell if this property is present.
      if ( unk_01 != 0x1 )
      {
        reader.BaseStream.Position -= 5;
        return;
      }

      for ( var i = 0; i < count; i++ )
      {
        _ = reader.ReadInt32(); // TODO
        _ = reader.ReadLengthPrefixedString32(); // TODO
      }

    }

    private void ReadLodRootsProperty( NativeReader reader, GeometryGraph graph, ISerializationContext context )
    {
      var serializer = new ObjectLodRootSerializer();
      graph.LodRoots = serializer.Deserialize( reader, context );
    }

    #endregion

    #region Data Read Methods

    private void ReadData( NativeReader reader, GeometryGraph graph, ISerializationContext context )
    {
      /* Reads the geometry data.
       * NOTE: Everything inside of here is subjectively named and interpreted.
       * As of writing this method, there is very little material to go on in terms
       * of RTTI or strings.
       * 
       * This data seems to be stored as streams and are accessed as needed.
       * 
       * RTTI of interest:
       *  - rendPROP_GLT
       *  - dsTYPE_STORAGE_ITEM_T<rendPROP_GLT>
       *  
       * Strings of interest:
       *  - OBJ_GEOM_STRM_BONE_INDEX
       *  - OBJ_GEOM_STRM_WEIGHT
       *  - OBJ_GEOM_STRM_FACE
       *  - OBJ_GEOM_STRM_VERT
       *  - OBJ_GEOM_STRM_INSTANCED
       *  - OBJ_GEOM_STRM_INTERLEAVED
       */
      while ( true )
      {
        var sentinel = reader.ReadUInt16();

        switch ( sentinel )
        {
          case GeometryGraphSentinels.Header:
            ReadHeaderData( reader, graph );
            break;
          case GeometryGraphSentinels.Buffers:
            ReadBufferData( reader, graph, context );
            break;
          case GeometryGraphSentinels.Meshes:
            ReadMeshData( reader, graph, context );
            break;
          case GeometryGraphSentinels.SubMeshes:
            ReadSubMeshData( reader, graph, context );
            break;
          case GeometryGraphSentinels.EndOfData:
            ReadEndOfData( reader, graph );
            return;
          default:
            FAIL( "Unknown GeometryGraph Data Sentinel: {0:X}", sentinel );
            break;
        }
      }
    }

    private void ReadHeaderData( NativeReader reader, GeometryGraph graph )
    {
      var endOffset = reader.ReadUInt32();

      graph.RootNodeIndex = reader.ReadInt16();
      graph.NodeCount = reader.ReadInt32();
      graph.BufferCount = reader.ReadInt32();
      graph.MeshCount = reader.ReadInt32();
      graph.SubMeshCount = reader.ReadInt32();

      var unk_01 = reader.ReadUInt32(); // TODO
      var unk_02 = reader.ReadUInt32(); // TODO

      ASSERT( reader.BaseStream.Position == endOffset,
          "Reader position does not match data header's end offset." );
    }

    private void ReadBufferData( NativeReader reader, GeometryGraph graph, ISerializationContext context )
    {
      var serializer = new GeometryBufferSerializer();
      graph.Buffers = serializer.Deserialize( reader, context );
    }

    private void ReadMeshData( NativeReader reader, GeometryGraph graph, ISerializationContext context )
    {
      var serializer = new GeometryMeshSerializer();
      graph.Meshes = serializer.Deserialize( reader, context );
    }

    private void ReadSubMeshData( NativeReader reader, GeometryGraph graph, ISerializationContext context )
    {
      var serializer = new GeometrySubMeshSerializer();
      graph.SubMeshes = serializer.Deserialize( reader, context );
    }

    private void ReadEndOfData( NativeReader reader, GeometryGraph graph )
    {
      var endOffset = reader.ReadUInt32();
      ASSERT( reader.BaseStream.Position == endOffset,
          "Reader position does not match data's end offset." );
    }

    #endregion

    #region Embedded Types

    private enum GeometryGraphType : ushort
    {
      Default = 1,
      Props = 3,
      Grass = 4
    }

    private class GeometryGraphSentinels
    {
      public const ushort Header = 0x0000;
      public const ushort Buffers = 0x0002;
      public const ushort Meshes = 0x0003;
      public const ushort SubMeshes = 0x0004;
      public const ushort EndOfData = 0xFFFF;
    }

    #endregion

  }

}
