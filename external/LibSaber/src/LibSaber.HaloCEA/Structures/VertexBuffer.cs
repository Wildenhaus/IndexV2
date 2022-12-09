using System.Numerics;
using LibSaber.Common;
using LibSaber.Extensions;
using LibSaber.HaloCEA.Enumerations;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  public class VertexBuffer : List<Vertex>
  {

    #region Properties

    public Vector3 Scale { get; private set; }
    public Vector3 Translation { get; private set; }

    #endregion

    #region Constructor

    public VertexBuffer()
    {
    }

    public VertexBuffer( int capacity )
      : base( capacity )
    {
    }

    #endregion

    #region Serialization

    public static VertexBuffer Deserialize( NativeReader reader, ISerializationContext context )
    {
      var vertexCount = reader.ReadInt32();
      if ( vertexCount == 0 )
        return new VertexBuffer( 0 );

      var buffer = new VertexBuffer( vertexCount );

      var parentObject = context.GetMostRecentObject<SaberObject>();
      var vertexIsCompressed = parentObject.GeometryFlags.HasFlag( ObjectGeometryFlags.CompressedVertex ); // TODO

      if ( vertexIsCompressed )
        ReadCompressedVertices( buffer, vertexCount, reader, context );
      else
        ReadUncompressedVertices( buffer, vertexCount, reader, context );

      return buffer;
    }

    private static void ReadCompressedVertices( VertexBuffer buffer, int vertexCount, NativeReader reader, ISerializationContext context )
    {
      var parentObject = context.GetMostRecentObject<SaberObject>();
      var normInVert4 = parentObject.GeometryFlags.HasFlag( ObjectGeometryFlags.NormInVert4 );

      var translation = buffer.Translation = new Vector3( reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16() );
      var scale = buffer.Scale = new Vector3( reader.ReadInt16(), reader.ReadInt16(), reader.ReadInt16() );

      for ( var i = 0; i < vertexCount; i++ )
      {
        var position = new Vector3(
            ( reader.ReadInt16().SNormToFloat() * scale.X ) + translation.X,
            ( reader.ReadInt16().SNormToFloat() * scale.Y ) + translation.Y,
            ( reader.ReadInt16().SNormToFloat() * scale.Z ) + translation.Z
          );

        Vector3 normal;
        if ( normInVert4 )
        {
          var w = reader.ReadInt16();
          normal = SaberMath.UnpackVector3FromInt16( w );
        }
        else
        {
          _ = reader.ReadInt16(); // TODO: This is tossing the W coord. Should we be doing something with it?
          normal = new Vector3( 1, 1, 1 ); // TODO: Is this correct?
        }

        var vertex = new Vertex( position, normal );
        buffer.Add( vertex );
      }
    }

    private static void ReadUncompressedVertices( VertexBuffer buffer, int vertexCount, NativeReader reader, ISerializationContext context )
    {
      buffer.Translation = new Vector3( 0 );
      buffer.Scale = new Vector3( 1 );

      for ( var i = 0; i < vertexCount; i++ )
      {
        var position = reader.ReadVector3();
        var vertex = new Vertex { Position = position };
        buffer.Add( vertex );
      }
    }

    #endregion

  }

}
