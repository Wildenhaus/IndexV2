using System.Numerics;
using LibSaber.Halo2A.Enumerations;
using LibSaber.Halo2A.IO;
using LibSaber.Halo2A.Serialization.Scripting;
using LibSaber.Halo2A.Structures;
using LibSaber.Halo2A.Structures.Materials;
using LibSaber.IO;
using LibSaber.Serialization;

namespace LibSaber.Halo2A.Serialization
{

  public sealed class GeometrySubMeshSerializer : H2ASerializerBase<List<GeometrySubMesh>>
  {

    #region Overrides

    public override List<GeometrySubMesh> Deserialize( NativeReader reader, ISerializationContext context )
    {
      var graph = context.GetMostRecentObject<GeometryGraph>();
      var sectionEndOffset = reader.ReadInt32();

      var count = graph.SubMeshCount;
      var submeshes = new List<GeometrySubMesh>( count );
      for ( var i = 0; i < count; i++ )
        submeshes.Add( new GeometrySubMesh() );

      var sentinelReader = new SentinelReader( reader );
      while ( reader.Position < sectionEndOffset )
      {
        sentinelReader.Next();
        switch ( sentinelReader.SentinelId )
        {
          case SubMeshSentinelIds.BufferInfo:
            ReadBufferInfo( reader, submeshes, sentinelReader.EndOffset );
            break;
          case SubMeshSentinelIds.MeshIds:
            ReadMeshIds( reader, submeshes );
            break;
          case SubMeshSentinelIds.Unk_02:
            ReadUnk02( reader, submeshes );
            break;
          case SubMeshSentinelIds.BoneMap:
            ReadBoneIds( reader, submeshes, context );
            break;
          case SubMeshSentinelIds.UvScaling:
            ReadUvScaling( reader, submeshes, context );
            break;
          case SubMeshSentinelIds.TransformInfo:
            ReadTransformInfo( reader, submeshes, context );
            break;
          case SubMeshSentinelIds.Materials_String:
            ReadMaterialsString( reader, submeshes );
            break;
          case SubMeshSentinelIds.Materials_Static:
            ReadMaterialsStatic( reader, submeshes );
            break;
          case SubMeshSentinelIds.Materials_Dynamic:
            ReadMaterialsDynamic( reader, submeshes );
            break;

          default:
            sentinelReader.ReportUnknownSentinel();
            break;
        }
      }

      ASSERT( reader.BaseStream.Position == sectionEndOffset,
          "Reader position does not match the submesh section's end offset." );

      return submeshes;
    }

    #endregion

    #region Private Methods

    private void ReadBufferInfo( NativeReader reader, List<GeometrySubMesh> submeshes, long endOffset )
    {
      // TODO: This is a bit hacky in order to determine whether or not we should do the seek nonsense.
      if ( submeshes.Count == 0 )
        return;

      const int BUFFER_INFO_SIZE = 0xC;
      var sectionSize = endOffset - reader.BaseStream.Position;
      var elementSize = sectionSize / submeshes.Count;
      bool hasAdditionalData = elementSize > BUFFER_INFO_SIZE;

      foreach ( var submesh in submeshes )
      {
        submesh.BufferInfo = new GeometrySubMeshBufferInfo
        {
          VertexOffset = reader.ReadUInt16(),
          VertexCount = reader.ReadUInt16(),
          FaceOffset = reader.ReadUInt16(),
          FaceCount = reader.ReadUInt16(),
          NodeId = reader.ReadInt16(),
          SkinCompoundId = reader.ReadInt16()
        };

        /* Not sure what this data is, but we're just gonna seek past it for now.
         * It has something to do with a flag set earlier in the file.
         * This implementation of looping 32-bits and doing a left rotate seems to be correct.
         * That's how it's done in the disassembly.
         */
        if ( hasAdditionalData )
        {
          var seekFlags = reader.ReadUInt32();

          for ( var i = 0; i < 32; i++ )
          {
            if ( ( seekFlags & 1 ) != 0 )
            {
              // These appear to be floats
              var a = reader.ReadFloat32();
              var b = reader.ReadFloat32();
            }
            seekFlags = ( seekFlags << 1 ) | ( seekFlags >> 31 );
          }
        }
      }
    }

    private void ReadMeshIds( NativeReader reader, List<GeometrySubMesh> submeshes )
    {
      foreach ( var submesh in submeshes )
        submesh.MeshId = reader.ReadInt32();
    }

    private void ReadUnk02( NativeReader reader, List<GeometrySubMesh> submeshes )
    {
      // TODO
      /* Not sure what this is. Just like the BufferInfo section, 
       * this does the same weird reading pattern at the end.
       * Seems to be a nested section and some scripting.
       */
      // TODO: Move this to own serializer once we figure out that it is

      foreach ( var submesh in submeshes )
      {
        var unk_01 = reader.ReadUInt16();
        var count = reader.ReadByte();

        while ( true )
        {
          var sentinel = reader.ReadUInt16();
          var endOffset = reader.ReadUInt32();

          switch ( sentinel )
          {
            case 0x0000:
            {
              for ( var i = 0; i < count; i++ )
              {
                _ = reader.ReadInt16();
                _ = reader.ReadInt16();
                _ = reader.ReadByte();
                _ = reader.ReadByte();
              }
            }
            break;
            case 0x0001:
            {
              for ( var i = 0; i < count; i++ )
              {
                _ = reader.ReadByte();
                _ = reader.ReadByte();
              }
            }
            break;
            case 0x0002:
            {
              for ( var i = 0; i < count; i++ )
              {
                _ = reader.ReadByte();
                _ = reader.ReadByte();
              }
            }
            break;
            case 0x0003:
            {
              for ( var i = 0; i < count; i++ )
                _ = reader.ReadLengthPrefixedString32();
            }
            break;
            case 0xFFFF:
              break;
            default:
              FAIL( $"Unknown sentinel: {0:X2}", sentinel );
              break;
          }

          ASSERT( reader.BaseStream.Position == endOffset );

          if ( sentinel == 0xFFFF )
            break;
        }

        var seekFlags = reader.ReadUInt32();
        for ( var i = 0; i < 32; i++ )
        {
          if ( ( seekFlags & 1 ) != 0 )
          {
            // These appear to be floats
            var a = reader.ReadFloat32();
            var b = reader.ReadFloat32();
          }
          seekFlags = ( seekFlags << 1 ) | ( seekFlags >> 31 );
        }

      }

    }

    private void ReadBoneIds( NativeReader reader, List<GeometrySubMesh> submeshes, ISerializationContext context )
    {
      var graph = context.GetMostRecentObject<GeometryGraph>();
      foreach ( var submesh in submeshes )
      {
        var mesh = graph.Meshes[ ( int ) submesh.MeshId ];
        if ( !mesh.Flags.HasFlag( GeometryMeshFlags.HasBoneIds ) )
          continue;

        var count = reader.ReadByte();
        var boneIds = submesh.BoneIds = new short[ count ];
        for ( var i = 0; i < count; i++ )
          boneIds[ i ] = reader.ReadInt16();
      }
    }

    private void ReadUvScaling( NativeReader reader, List<GeometrySubMesh> submeshes, ISerializationContext context )
    {
      var graph = context.GetMostRecentObject<GeometryGraph>();
      foreach ( var submesh in submeshes )
      {
        var mesh = graph.Meshes[ ( int ) submesh.MeshId ];
        if ( !mesh.Flags.HasFlag( GeometryMeshFlags.HasUvScaling ) )
          continue;

        var count = reader.ReadByte();
        var data = submesh.UvScaling = new Dictionary<byte, short>( count );
        for ( var i = 0; i < count; i++ )
        {
          var uvSetIndex = reader.ReadByte();
          var uvScale = reader.ReadInt16();
          data[ uvSetIndex ] = uvScale;
        }
      }
    }

    private void ReadTransformInfo( NativeReader reader, List<GeometrySubMesh> submeshes, ISerializationContext context )
    {
      // TODO:
      /* This is probably transform data, but sometimes it behaves inconsistently.
       */

      var graph = context.GetMostRecentObject<GeometryGraph>();
      foreach ( var submesh in submeshes )
      {
        var mesh = graph.Meshes[ submesh.MeshId ];
        if ( !mesh.Flags.HasFlag( GeometryMeshFlags.HasTransformInfo ) )
          continue;

        submesh.Position = new Vector3(
          x: reader.ReadInt16(),
          y: reader.ReadInt16(),
          z: reader.ReadInt16()
        );

        // TODO: Are int16s correct?
        submesh.Scale = new Vector3(
          x: reader.ReadInt16(),
          y: reader.ReadInt16(),
          z: reader.ReadInt16()
        );
      }
    }

    /* The next three properties are all for materials. The material data itself is always structured
     * the same (see the material serializers). However, for whatever reason, the data can be represented
     * in three different ways:
     * 
     *  - Materials (String):  All of the data is stored in a pascal string. It seems to be formatted similarly
     *                         to their scripting language, which is somewhat similar to JSON.
     *                        
     *  - Materials (Static):  Each property starts with a name (null-terminated string), followed by an unknown
     *                         32-bit int, followed by the data type (see the enum in the serializer), followed
     *                         by the property's value.
     *                        
     *  - Materials (Dynamic): The most commonly used form.
     *                         Each property starts with a name (pascal string) followed by the data type
     *                         (see the enum in the serializer), followed by the property's value.
     *                         
     *  They just couldn't make it simple, could they?
     */

    private void ReadMaterialsString( NativeReader reader, List<GeometrySubMesh> submeshes )
    {
      /* Materials with this sentinel are represented as just a string.
       */
      var serializer = new StringScriptingSerializer<SaberMaterial>();
      foreach ( var submesh in submeshes )
      {
        submesh.NodeId = reader.ReadUInt16();
        submesh.Material = serializer.Deserialize( reader );
      }
    }

    private void ReadMaterialsStatic( NativeReader reader, List<GeometrySubMesh> submeshes )
    {
      /* Materials with this sentinel are similar to the MaterialsDynamic section, but the strings 
       * are null-terminated and there's an additional int32 after the property name.
       */
      var serializer = new StaticBinaryScriptingSerializer<SaberMaterial>();
      foreach ( var submesh in submeshes )
      {
        submesh.NodeId = reader.ReadUInt16();
        submesh.Material = serializer.Deserialize( reader );
      }
    }

    private void ReadMaterialsDynamic( NativeReader reader, List<GeometrySubMesh> submeshes )
    {
      /* This is the most common form of materials.
       * The properties are pascal strings, followed by the data type, and then the value.
       */
      var serializer = new DynamicBinaryScriptingSerializer<SaberMaterial>();
      foreach ( var submesh in submeshes )
      {
        submesh.NodeId = reader.ReadUInt16();
        submesh.Material = serializer.Deserialize( reader );
      }
    }

    #endregion

    #region SubMesh Sentinel Ids

    private class SubMeshSentinelIds
    {
      public const short BufferInfo = 0x0000;
      public const short MeshIds = 0x0001;
      public const short Unk_02 = 0x0002;
      public const short BoneMap = 0x0003;
      public const short UvScaling = 0x0004;
      public const short TransformInfo = 0x0005;
      public const short Materials_String = 0x0006;
      public const short Materials_Static = 0x0007;
      public const short Materials_Dynamic = 0x0008;
    }

    #endregion

  }

}
