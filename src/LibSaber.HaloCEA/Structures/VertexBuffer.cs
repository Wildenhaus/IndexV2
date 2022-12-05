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

      var translationTransform = Vector3<short>.Deserialize( reader, context );
      var scaleTransform = Vector3<short>.Deserialize( reader, context );

      for ( var i = 0; i < vertexCount; i++ )
      {
        var position = new Vector3<float>(
          ( reader.ReadInt16().SNormToFloat() * scaleTransform.X ) + translationTransform.X,
          ( reader.ReadInt16().SNormToFloat() * scaleTransform.Y ) + translationTransform.Y,
          ( reader.ReadInt16().SNormToFloat() * scaleTransform.Z ) + translationTransform.Z
          );

        Vector3<float> normal;
        if ( normInVert4 )
        {
          var w = reader.ReadInt16();
          normal = SaberMath.UnpackVector3FromInt16( w );
        }
        else
        {
          _ = reader.ReadInt16(); // TODO: This is tossing the W coord. Should we be doing something with it?
          normal = new Vector3<float>( 1, 1, 1 ); // TODO: Is this correct?
        }

        var vertex = new Vertex( position, normal );
        buffer.Add( vertex );
      }
    }

    private static void ReadUncompressedVertices( VertexBuffer buffer, int vertexCount, NativeReader reader, ISerializationContext context )
    {
      for ( var i = 0; i < vertexCount; i++ )
      {
        var position = Vector3<float>.Deserialize( reader, context );
        var vertex = new Vertex { Position = position };
        buffer.Add( vertex );
      }
    }

    #endregion

  }

}
