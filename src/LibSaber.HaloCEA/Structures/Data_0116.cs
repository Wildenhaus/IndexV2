using LibSaber.HaloCEA.Enumerations;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  [Sentinel( SentinelIds.ObjectSkinningData )]
  public struct Data_0116
  {

    #region Data Members

    // vertex bind data?

    public int Sentinel_0117_00; // bone count?
    public int ElementSize; // Element Size

    public List<short> Sentinel_0118;
    public byte[] Sentinel_0119;
    //public int[] Sentinel_011A;
    public Vector4<float>[] BoneWeights;
    public (int, int)[] Sentinel_011B;

    public short Sentinel_0133_00;
    public short Sentinel_0133_01;
    //public byte[] Sentinel_0133_02;
    public Vector4<byte>[] BoneIds;

    #endregion

    #region Serialization

    public static Data_0116 Deserialize( NativeReader reader, ISerializationContext context )
    {
      var obj = context.GetMostRecentObject<SaberObject>();
      var data = new Data_0116();

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          case SentinelIds.Sentinel_0117:
          {
            data.Sentinel_0117_00 = reader.ReadInt32();
            data.ElementSize = reader.ReadInt32();
            break;
          }
          case SentinelIds.Sentinel_0118:
          {
            var count = data.Sentinel_0117_00;
            var list = data.Sentinel_0118 = new List<short>( count );
            for ( var i = 0; i < count; i++ )
              list.Add( reader.ReadInt16() );

            break;
          }
          case SentinelIds.Sentinel_0119:
          {
            var count = obj.ObjectInfo.VertexCount * data.ElementSize;
            var list = data.Sentinel_0119 = new byte[ count ];
            reader.Read( list );

            break;
          }
          case SentinelIds.Sentinel_011A:
          {
            // vert blend weights?
            //var count = obj.ObjectInfo.VertexCount;
            //var buffer = data.BoneWeights = new int[ count ];

            //if ( obj.GeometryFlags[ 5 ] )
            //  for ( var i = 0; i < count; i++ )
            //    buffer[ i ] = reader.ReadByte();
            //else
            //  for ( var i = 0; i < count; i++ )
            //    buffer[ i ] = reader.ReadInt32();
            var count = obj.ObjectInfo.VertexCount;
            var buffer = data.BoneWeights = new Vector4<float>[ count ];

            if ( obj.GeometryFlags.HasFlag( ObjectGeometryFlags.UnkHasSingleWeight ) )
            {
              for ( var i = 0; i < count; i++ )
              {
                var value = reader.ReadByte() / 255.0f;
                buffer[ i ] = new Vector4<float>( value, value, value, value );
              }
            }
            else
            {
              for ( var i = 0; i < count; i++ )
              {
                var weight1 = reader.ReadByte() / 255.0f;
                var weight2 = reader.ReadByte() / 255.0f;
                var weight3 = reader.ReadByte() / 255.0f;
                var weight4 = reader.ReadByte() / 255.0f;
                buffer[ i ] = new Vector4<float>( weight1, weight2, weight3, weight4 );
              }
            }

            break;
          }
          case SentinelIds.Sentinel_011B:
          {
            var count = reader.ReadInt32();

            var buffer = data.Sentinel_011B = new (int, int)[ count ];
            for ( var i = 0; i < count; i++ )
              buffer[ i ] = (reader.ReadInt32(), reader.ReadInt32());

            break;
          }
          case SentinelIds.Sentinel_0133:
          {
            data.Sentinel_0133_00 = reader.ReadInt16();
            data.Sentinel_0133_01 = reader.ReadInt16();

            //var count = obj.ObjectInfo.VertexCount * data.ElementSize;
            //var buffer = data.Sentinel_0133_02 = new byte[ count ]; // bone ids?
            //reader.ReadBytes( buffer );
            var count = obj.ObjectInfo.VertexCount;
            var buffer = data.BoneIds = new Vector4<byte>[ count ]; // bone ids?
            for ( var i = 0; i < count; i++ )
              buffer[ i ] = Vector4<byte>.Deserialize( reader, context );

            break;
          }

          case SentinelIds.Delimiter:
            return data;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return data;
    }

    #endregion

  }

}
