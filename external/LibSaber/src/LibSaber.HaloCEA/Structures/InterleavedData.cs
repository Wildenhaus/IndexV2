using LibSaber.Extensions;
using LibSaber.HaloCEA.Enumerations;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  public struct InterleavedData
  {

    #region Data Members

    public Vector4<float>? Tangent0;
    public Vector4<float>? Tangent1;
    public Vector4<float>? Tangent2;
    public Vector4<float>? Tangent3;

    public Vector4<byte>? Color0;
    public Vector4<byte>? Color1;
    public Vector4<byte>? Color2;

    public Vector3<float>? UV0;
    public Vector3<float>? UV1;
    public Vector3<float>? UV2;
    public Vector3<float>? UV3;

    public float? UnkUvDeltaU;
    public float? UnkUvDeltaV;

    #endregion

    #region Serialization

    public static InterleavedData Deserialize( NativeReader reader, ISerializationContext context )
    {
      var buffer = context.GetMostRecentObject<InterleavedDataBuffer>();
      var flags = buffer.Flags;

      var data = new InterleavedData();

      ReadTangents( ref data, reader, flags );
      ReadColors( ref data, reader, flags );
      ReadUVs( ref data, reader, flags );

      return data;
    }

    private static void ReadTangents( ref InterleavedData data, NativeReader reader, BitSet<short> flags )
    {
      if ( flags[ InterleavedDataFlags._TANG0 ] )
        data.Tangent0 = ReadTangent( reader, flags[ InterleavedDataFlags._COMPRESSED_TANG_0 ] );
      if ( flags[ InterleavedDataFlags._TANG1 ] )
        data.Tangent1 = ReadTangent( reader, flags[ InterleavedDataFlags._COMPRESSED_TANG_1 ] );
      if ( flags[ InterleavedDataFlags._TANG2 ] )
        data.Tangent2 = ReadTangent( reader, flags[ InterleavedDataFlags._COMPRESSED_TANG_2 ] );
      if ( flags[ InterleavedDataFlags._TANG3 ] )
        data.Tangent3 = ReadTangent( reader, flags[ InterleavedDataFlags._COMPRESSED_TANG_3 ] );
    }

    private static Vector4<float> ReadTangent( NativeReader reader, bool isCompressed )
    {
      if ( isCompressed )
      {
        var x = reader.ReadSByte().SNormToFloat();
        var y = reader.ReadSByte().SNormToFloat();
        var z = reader.ReadSByte().SNormToFloat();
        var w = reader.ReadSByte().SNormToFloat();

#if DEBUG
        ASSERT( x >= -1.01 && x <= 1.01, "Tangent X coord out of bounds." );
        ASSERT( y >= -1.01 && y <= 1.01, "Tangent Y coord out of bounds." );
        ASSERT( z >= -1.01 && z <= 1.01, "Tangent Z coord out of bounds." );
        ASSERT( w >= -1.01 && w <= 1.01, "Tangent W coord out of bounds." );
#endif

        return new Vector4<float>( x, y, z, w );
      }
      else
      {
        var x = reader.ReadFloat32();
        var y = reader.ReadFloat32();
        var z = reader.ReadFloat32();
        var w = reader.ReadFloat32();

        return new Vector4<float>( x, y, z, w );
      }
    }

    private static void ReadColors( ref InterleavedData data, NativeReader reader, BitSet<short> flags )
    {
      if ( flags[ InterleavedDataFlags._COL0 ] )
        data.Color0 = ReadColor( reader, flags );
      if ( flags[ InterleavedDataFlags._COL1 ] )
        data.Color1 = ReadColor( reader, flags );
      if ( flags[ InterleavedDataFlags._COL2 ] )
        data.Color2 = ReadColor( reader, flags );
    }

    private static Vector4<byte> ReadColor( NativeReader reader, BitSet<short> flags )
    {
      var r = reader.ReadByte();
      var g = reader.ReadByte();
      var b = reader.ReadByte();
      var a = reader.ReadByte();

      return new Vector4<byte>( r, g, b, a );
    }

    private static void ReadUVs( ref InterleavedData data, NativeReader reader, BitSet<short> flags )
    {
      if ( flags[ InterleavedDataFlags._TEX0 ] )
        data.UV0 = ReadUV( reader, flags[ InterleavedDataFlags._COMPRESSED_TEX_0 ] );
      if ( flags[ InterleavedDataFlags._TEX1 ] )
        data.UV1 = ReadUV( reader, flags[ InterleavedDataFlags._COMPRESSED_TEX_1 ] );
      if ( flags[ InterleavedDataFlags._TEX2 ] )
        data.UV2 = ReadUV( reader, flags[ InterleavedDataFlags._COMPRESSED_TEX_2 ] );
      if ( flags[ InterleavedDataFlags._TEX3 ] )
        data.UV3 = ReadUV( reader, flags[ InterleavedDataFlags._COMPRESSED_TEX_3 ] );

      if ( flags[ InterleavedDataFlags.Unk_0A ] )
        data.UnkUvDeltaU = reader.ReadInt16().SNormToFloat();
      if ( flags[ InterleavedDataFlags.Unk_0B ] )
        data.UnkUvDeltaV = reader.ReadInt16().SNormToFloat();
    }

    private static Vector3<float> ReadUV( NativeReader reader, bool isCompressed )
    {
      if ( isCompressed )
      {
        var u = reader.ReadInt16().SNormToFloat();
        var v = 1 - reader.ReadInt16().SNormToFloat();

        return new Vector3<float>( u, v, 0 );
      }
      else
      {
        var u = reader.ReadFloat32();
        var v = 1 - reader.ReadFloat32();

        return new Vector3<float>( u, v, 0 );
      }
    }

    #endregion

  }

}
