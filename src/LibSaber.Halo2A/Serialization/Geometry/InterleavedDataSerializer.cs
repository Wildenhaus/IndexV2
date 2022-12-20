using System.Numerics;
using LibSaber.Extensions;
using LibSaber.Halo2A.Enumerations;
using LibSaber.Halo2A.Structures;
using LibSaber.Halo2A.Structures.Geometry;
using LibSaber.IO;

namespace LibSaber.Halo2A.Serialization.Geometry
{

  public sealed class InterleavedDataSerializer : GeometryElementSerializer<InterleavedData>
  {

    #region Constructor

    public InterleavedDataSerializer( GeometryBuffer buffer )
      : base( buffer )
    {
    }

    #endregion

    #region Overrides

    public override InterleavedData Deserialize( NativeReader reader )
    {
      var startPos = reader.Position;
      var endPos = reader.Position + Buffer.ElementSize;

      var data = new InterleavedData();

      ReadTangents( reader, ref data );
      ReadVertexColors( reader, ref data );
      ReadUVs( reader, ref data );

      var readSize = reader.Position - startPos;
      ASSERT( Buffer.ElementSize == readSize );

      return data;
    }

    public override IEnumerable<InterleavedData> DeserializeRange( NativeReader reader, int startIndex, int endIndex )
    {
      var startOffset = Buffer.StartOffset + ( startIndex * Buffer.ElementSize );
      var length = endIndex - startIndex;

      reader.Position = startOffset;

      for ( var i = 0; i < length; i++ )
        yield return Deserialize( reader );
    }

    #endregion

    #region Private Methods

    private void ReadTangents( NativeReader reader, ref InterleavedData data )
    {
      if ( Flags.HasFlag( GeometryBufferFlags._TANG0 ) )
        data.Tangent0 = ReadTangent( reader );
      if ( Flags.HasFlag( GeometryBufferFlags._TANG1 ) )
        data.Tangent1 = ReadTangent( reader );
      if ( Flags.HasFlag( GeometryBufferFlags._TANG2 ) )
        data.Tangent2 = ReadTangent( reader );
      if ( Flags.HasFlag( GeometryBufferFlags._TANG3 ) )
        data.Tangent3 = ReadTangent( reader );
      if ( Flags.HasFlag( GeometryBufferFlags._TANG4 ) )
        data.Tangent4 = ReadTangent( reader );
    }

    private Vector4 ReadTangent( NativeReader reader )
    {
      var x = reader.ReadSByte().SNormToFloat();
      var y = reader.ReadSByte().SNormToFloat();
      var z = reader.ReadSByte().SNormToFloat();
      var w = reader.ReadSByte().SNormToFloat();

      ASSERT( x >= -1.01 && x <= 1.01, "Tangent X coord out of bounds." );
      ASSERT( y >= -1.01 && y <= 1.01, "Tangent Y coord out of bounds." );
      ASSERT( z >= -1.01 && z <= 1.01, "Tangent Z coord out of bounds." );
      ASSERT( w >= -1.01 && w <= 1.01, "Tangent W coord out of bounds." );

      return new Vector4( x, y, z, w );
    }

    private void ReadVertexColors( NativeReader reader, ref InterleavedData data )
    {
      if ( Flags.HasFlag( GeometryBufferFlags._COLOR0 ) )
        data.Color0 = ReadVertexColor( reader );
      if ( Flags.HasFlag( GeometryBufferFlags._COLOR1 ) )
        data.Color1 = ReadVertexColor( reader );
      if ( Flags.HasFlag( GeometryBufferFlags._COLOR2 ) )
        data.Color2 = ReadVertexColor( reader );
    }

    private Vector4 ReadVertexColor( NativeReader reader )
    {
      var r = reader.ReadByte() / 255.0f;
      var g = reader.ReadByte() / 255.0f;
      var b = reader.ReadByte() / 255.0f;
      var a = reader.ReadByte() / 255.0f;

      return new Vector4( r, g, b, a );
    }

    private void ReadUVs( NativeReader reader, ref InterleavedData data )
    {
      if ( Flags.HasFlag( GeometryBufferFlags._TEX0 ) )
        data.UV0 = ReadUV( reader, Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_0 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX1 ) )
        data.UV1 = ReadUV( reader, Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_1 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX2 ) )
        data.UV2 = ReadUV( reader, Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_2 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX3 ) )
        data.UV3 = ReadUV( reader, Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_3 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX4 ) )
        data.UV4 = ReadUV( reader, Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_4 ) );
    }

    private Vector4 ReadUV( NativeReader reader, bool isCompressed )
    {
      if ( isCompressed )
      {
        // Fairly sure this is correct
        var u = reader.ReadInt16().SNormToFloat();
        var v = 1 - reader.ReadInt16().SNormToFloat();

        return new Vector4( u, v, 0, 0 );
      }
      else
      {
        // Fairly sure this is correct
        var u = reader.ReadFloat32();
        var v = 1 - reader.ReadFloat32();

        return new Vector4( u, v, 0, 0 );
      }
    }

    #endregion

  }

}
