using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
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

    public InterleavedDataSerializer( NativeReader reader, GeometryBuffer buffer )
      : base( reader, buffer )
    {
    }

    #endregion

    #region Overrides

    protected override InterleavedData ReadElement()
    {
      var data = new InterleavedData();

      ReadTangents( ref data );
      ReadVertexColors( ref data );
      ReadUVs( ref data );

      return data;
    }

    #endregion

    #region Private Methods

    private void ReadTangents( ref InterleavedData data )
    {
      if ( Flags.HasFlag( GeometryBufferFlags._TANG0 ) )
        data.Tangent0 = ReadTangent();
      if ( Flags.HasFlag( GeometryBufferFlags._TANG1 ) )
        data.Tangent1 = ReadTangent();
      if ( Flags.HasFlag( GeometryBufferFlags._TANG2 ) )
        data.Tangent2 = ReadTangent();
      if ( Flags.HasFlag( GeometryBufferFlags._TANG3 ) )
        data.Tangent3 = ReadTangent();
      if ( Flags.HasFlag( GeometryBufferFlags._TANG4 ) )
        data.Tangent4 = ReadTangent();
    }

    private Vector4 ReadTangent()
    {
      var x = Reader.ReadSByte().SNormToFloat();
      var y = Reader.ReadSByte().SNormToFloat();
      var z = Reader.ReadSByte().SNormToFloat();
      var w = Reader.ReadSByte().SNormToFloat();

      ASSERT( x >= -1.01 && x <= 1.01, "Tangent X coord out of bounds." );
      ASSERT( y >= -1.01 && y <= 1.01, "Tangent Y coord out of bounds." );
      ASSERT( z >= -1.01 && z <= 1.01, "Tangent Z coord out of bounds." );
      ASSERT( w >= -1.01 && w <= 1.01, "Tangent W coord out of bounds." );

      return new Vector4( x, y, z, w );
    }

    private void ReadVertexColors( ref InterleavedData data )
    {
      if ( Flags.HasFlag( GeometryBufferFlags._COLOR0 ) )
        data.Color0 = ReadVertexColor();
      if ( Flags.HasFlag( GeometryBufferFlags._COLOR1 ) )
        data.Color1 = ReadVertexColor();
      if ( Flags.HasFlag( GeometryBufferFlags._COLOR2 ) )
        data.Color2 = ReadVertexColor();
    }

    private Vector4 ReadVertexColor()
    {
      var r = Reader.ReadByte() / 255.0f;
      var g = Reader.ReadByte() / 255.0f;
      var b = Reader.ReadByte() / 255.0f;
      var a = Reader.ReadByte() / 255.0f;

      return new Vector4( r, g, b, a );
    }

    private void ReadUVs( ref InterleavedData data )
    {
      if ( Flags.HasFlag( GeometryBufferFlags._TEX0 ) )
        data.UV0 = ReadUV( Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_0 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX1 ) )
        data.UV1 = ReadUV( Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_1 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX2 ) )
        data.UV2 = ReadUV( Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_2 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX3 ) )
        data.UV3 = ReadUV( Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_3 ) );
      if ( Flags.HasFlag( GeometryBufferFlags._TEX4 ) )
        data.UV4 = ReadUV( Flags.HasFlag( GeometryBufferFlags._COMPRESSED_TEX_4 ) );
    }

    private Vector4 ReadUV( bool isCompressed )
    {
      if ( isCompressed )
      {
        // Fairly sure this is correct
        var u = Reader.ReadInt16().SNormToFloat();
        var v = 1 - Reader.ReadInt16().SNormToFloat();

        return new Vector4( u, v, 0, 0 );
      }
      else
      {
        // Fairly sure this is correct
        var u = Reader.ReadFloat32();
        var v = 1 - Reader.ReadFloat32();

        return new Vector4( u, v, 0, 0 );
      }
    }

    #endregion

  }

}
