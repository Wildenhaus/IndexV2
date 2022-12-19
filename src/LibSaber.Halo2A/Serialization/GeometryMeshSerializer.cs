using LibSaber.Halo2A.IO;
using LibSaber.Halo2A.Structures;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class GeometryMeshSerializer : H2ASerializerBase<List<GeometryMesh>>
  {

    #region Overrides

    public override List<GeometryMesh> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var graph = context.GetMostRecentObject<GeometryGraph>();
      var sectionEndOffset = reader.ReadInt32();

      var count = graph.MeshCount;
      var meshes = new List<GeometryMesh>();
      for ( var i = 0; i < count; i++ )
        meshes.Add( new GeometryMesh() );

      var sentinelReader = new SentinelReader( reader );
      while ( reader.Position < sectionEndOffset )
      {
        sentinelReader.Next();
        switch ( sentinelReader.SentinelId )
        {
          case MeshSentinelIds.Flags:
            ReadMeshFlags( reader, meshes );
            break;
          case MeshSentinelIds.BufferInfo:
            ReadBufferInfo( reader, meshes );
            break;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      ASSERT( reader.Position == sectionEndOffset,
          "Reader position does not match the mesh section's end offset." );

      return meshes;
    }

    #endregion

    #region Private Methods

    private void ReadMeshFlags( NativeReader reader, List<GeometryMesh> meshes )
    {
      foreach ( var mesh in meshes )
        mesh.Flags = BitSet<short>.Deserialize( reader, null );
    }

    private void ReadBufferInfo( NativeReader reader, List<GeometryMesh> meshes )
    {
      foreach ( var mesh in meshes )
      {
        var count = reader.ReadByte();
        var buffers = new GeometryMeshBufferInfo[ count ];

        for ( var i = 0; i < count; i++ )
        {
          buffers[ i ] = new GeometryMeshBufferInfo
          {
            BufferId = reader.ReadInt32(),
            SubBufferOffset = reader.ReadInt32()
          };
        }

        mesh.Buffers = buffers;
      }
    }

    #endregion

    #region Mesh Sentinel Ids

    private class MeshSentinelIds
    {
      public const short Flags = 0x0000;
      public const short BufferInfo = 0x0002;
    }

    #endregion

  }

}
