using System.Diagnostics;
using System.Text;
using LibSaber.IO;
using LibSaber.Serialization;
using LibSaber.Shared.Attributes;
using LibSaber.Shared.Structures;

namespace LibSaber.HaloCEA.Structures
{

  public class SaberObject
  {

    #region Data Members

    // Global Sentinels =================================================
    [Sentinel( SentinelIds.ObjectInfo )]
    public Data_03B9 ObjectInfo;

    [Sentinel( SentinelIds.ObjectSubmeshData )]
    public Data_0107 SubmeshData;

    [Sentinel( SentinelIds.ObjectBoundingBox )]
    public Box? BoundingBox;

    [Sentinel( SentinelIds.ObjectAffixes )]
    public string? Affixes;

    [Sentinel( SentinelIds.ObjectSkinningData )]
    public Data_0116? SkinningData;

    [Sentinel( SentinelIds.Sentinel_0123 )]
    public bool Data_0123;

    [Sentinel( SentinelIds.ObjectSharingObjectInfo )]
    public Data_0129 SharingObjectInfo;

    [Sentinel( SentinelIds.Sentinel_012A )]
    public bool Data_012A;

    [Sentinel( SentinelIds.ObjectParentId )]
    public int? ParentId;

    [Sentinel( SentinelIds.Sentinel_012E )]
    public BitSet<short> GeometryFlags;

    [Sentinel( SentinelIds.ObjectUvScaling )]
    public UvScalingMap? UvScaling;

    [Sentinel( SentinelIds.ObjectInterleavedBuffer )]
    public InterleavedDataBuffer? InterleavedDataBuffer;

    [Sentinel( SentinelIds.Sentinel_0131 )]
    public bool Data_0131;

    [Sentinel( SentinelIds.Sentinel_0135 )]
    public Data_0135 TranslationVectors;

    [Sentinel( SentinelIds.Sentinel_0483 )]
    public int Data_0483;

    // Object Sentinels =================================================
    [Sentinel( SaberObjectSentinelIds.ObjectVertexBuffer )]
    public VertexBuffer Vertices;

    [Sentinel( SaberObjectSentinelIds.ObjectFaceBuffer )]
    public FaceBuffer Faces;

    [Sentinel( SaberObjectSentinelIds.ObjectMaterialColor )]
    public Vector4<byte>? MaterialColor;

    [Sentinel( SaberObjectSentinelIds.ObjectMatrix )]
    public Matrix4<float>? Matrix;

    [Sentinel( SaberObjectSentinelIds.Sentinel_00FA )]
    public int BoneId; // Bone ID?

    [Sentinel( SaberObjectSentinelIds.Sentinel_00FD )]
    public string? UnkScripting; // Scripting?

    #endregion

    #region Serialization

    public static SaberObject Deserialize( NativeReader reader, ISerializationContext context )
    {
      var obj = new SaberObject();
      context.AddObject( obj );

      var sentinelReader = new SentinelReader( reader );
      while ( sentinelReader.Next() )
      {
        switch ( sentinelReader.SentinelId )
        {
          // Global Sentinels =================================================
          case SentinelIds.ObjectInfo:
          {
            obj.ObjectInfo = Data_03B9.Deserialize( reader, context );
            break;
          }
          case SentinelIds.ObjectSubmeshData:
          {
            obj.SubmeshData = Data_0107.Deserialize( reader, context );
            break;
          }
          case SentinelIds.ObjectBoundingBox:
          {
            _ = reader.ReadInt32();
            obj.BoundingBox = Box.Deserialize( reader, context );
            break;
          }
          case SentinelIds.ObjectAffixes:
          {
            obj.Affixes = reader.ReadNullTerminatedString();
            break;
          }
          case SentinelIds.ObjectSkinningData:
          {
            obj.SkinningData = Data_0116.Deserialize( reader, context );
            break;
          }
          case SentinelIds.Sentinel_0123:
          {
            // Not sure what this is... it's empty. Sets flags?
            obj.Data_0123 = true;
            break;
          }
          case SentinelIds.ObjectSharingObjectInfo:
          {
            obj.SharingObjectInfo = Data_0129.Deserialize( reader, context );
            break;
          }
          case SentinelIds.Sentinel_012A:
          {
            // Not sure what this is... it's empty. Sets flags?
            obj.Data_012A = true;
            break;
          }
          case SentinelIds.ObjectParentId:
          {
            obj.ParentId = reader.ReadInt32();
            break;
          }
          case SentinelIds.Sentinel_012E:
          {
            obj.GeometryFlags = BitSet<short>.Deserialize( reader, context );
            break;
          }
          case SentinelIds.ObjectUvScaling:
          {
            obj.UvScaling = UvScalingMap.Deserialize( reader, context );
            break;
          }
          case SentinelIds.ObjectInterleavedBuffer:
          {
            obj.InterleavedDataBuffer = Structures.InterleavedDataBuffer.Deserialize( reader, context );
            break;
          }
          case SentinelIds.Sentinel_0131:
          {
            // Not sure what this is... it's empty. Sets flags?
            obj.Data_0131 = true;
            break;
          }
          case SentinelIds.Sentinel_0135:
          {
            // Disassembly suggests this will only be present if ObjectGeometryFlags.CompressedVertex is set
            obj.TranslationVectors = Data_0135.Deserialize( reader, context );
            break;
          }
          case SentinelIds.Sentinel_0483:
          {
            obj.Data_0483 = reader.ReadInt32();
            break;
          }

          // Object Sentinels =================================================
          case SaberObjectSentinelIds.ObjectVertexBuffer:
          {
            obj.Vertices = VertexBuffer.Deserialize( reader, context );
            break;
          }
          case SaberObjectSentinelIds.ObjectFaceBuffer:
          {
            obj.Faces = FaceBuffer.Deserialize( reader, context );
            break;
          }
          case SaberObjectSentinelIds.ObjectMaterialColor:
          {
            obj.MaterialColor = Vector4<byte>.Deserialize( reader, context );
            break;
          }
          case SaberObjectSentinelIds.ObjectMatrix:
          {
            obj.Matrix = Matrix4<float>.Deserialize( reader, context );
            break;
          }
          case SaberObjectSentinelIds.Sentinel_00FA:
          {
            obj.BoneId = reader.ReadInt32();
            break;
          }
          case SaberObjectSentinelIds.Sentinel_00FD:
          {
            var innerSentinelReader = new SentinelReader( reader );
            innerSentinelReader.Next();

            ASSERT( innerSentinelReader.SentinelId == SentinelIds.Sentinel_01BA,
              "Unexpected Sentinel inside Object Sentinel 00FD: {0:X4}",
              innerSentinelReader.SentinelId );

            obj.UnkScripting = reader.ReadNullTerminatedString();
            break;
          }

          // Default Sentinels ================================================
          case SentinelIds.Delimiter:
            return obj;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      return obj;
    }

    #endregion

    #region CEAObject Sentinels

    private static class SaberObjectSentinelIds
    {
      public const SentinelId Sentinel_00F0 = 0x00F0;
      public const SentinelId ObjectVertexBuffer = 0x00F1;
      public const SentinelId ObjectFaceBuffer = 0x00F2;
      public const SentinelId Sentinel_00F3 = 0x00F3;
      public const SentinelId Sentinel_00F4 = 0x00F4;
      public const SentinelId Sentinel_00F5 = 0x00F5;
      public const SentinelId Sentinel_00F6 = 0x00F6;
      public const SentinelId Sentinel_00F7 = 0x00F7;
      public const SentinelId ObjectMaterialColor = 0x00F8;
      public const SentinelId ObjectMatrix = 0x00F9;
      public const SentinelId Sentinel_00FA = 0x00FA;
      public const SentinelId Sentinel_00FB = 0x00FB;
      public const SentinelId Sentinel_00FC = 0x00FC;
      public const SentinelId Sentinel_00FD = 0x00FD;
    }

    #endregion

  }
}
