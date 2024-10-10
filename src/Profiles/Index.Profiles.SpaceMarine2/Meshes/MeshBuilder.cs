using System.Diagnostics;
using System.Numerics;
using Assimp;
using Index.Profiles.SpaceMarine2.Common;
using LibSaber.IO;
using LibSaber.SpaceMarine2.Enumerations;
using LibSaber.SpaceMarine2.Serialization.Geometry;
using LibSaber.SpaceMarine2.Structures;
using LibSaber.SpaceMarine2.Structures.Geometry;
using Microsoft.EntityFrameworkCore;

namespace Index.Profiles.SpaceMarine2.Meshes
{

  public class MeshBuilder
  {

    #region Properties

    protected SceneContext Context { get; }
    protected objOBJ Object { get; }
    protected GeometrySubMesh Submesh { get; }

    private Stream Stream => Context.Stream;
    private NativeReader Reader => Context.Reader;
    private objGEOM_MNG Graph => Context.GeometryGraph;

    public Mesh Mesh { get; }
    public short SkinCompoundId { get; }
    protected Dictionary<short, Bone> Bones { get; }
    protected Dictionary<string, Bone> BoneNames { get; }
    protected Dictionary<int, int> VertexLookup { get; }

    #endregion

    #region Constructor

    public MeshBuilder( SceneContext context, objOBJ obj, GeometrySubMesh submesh )
    {
      //if ( obj.GetName() == "acheran_body" )
      //  Debugger.Break();

      Context = context;
      Object = obj;
      Submesh = submesh;
      SkinCompoundId = Submesh.BufferInfo.SkinCompoundId;

      var meshName = Object.GetName();
      Mesh = new Mesh( meshName, PrimitiveType.Triangle );

      Bones = new Dictionary<short, Bone>();
      BoneNames = new Dictionary<string, Bone>();
      VertexLookup = new Dictionary<int, int>();
    }

    #endregion

    #region Public Methods

    public Mesh Build()
    {
      var meshData = Graph.Meshes[ Submesh.MeshId ];
      foreach ( var meshBuffer in meshData.Buffers )
      {
        var buffer = Graph.Buffers[ meshBuffer.BufferId ];

        switch ( buffer.ElementType )
        {
          case GeometryElementType.Vertex:
            AddVertices( buffer, meshBuffer );
            break;
          case GeometryElementType.Face:
            if ( buffer.ElementSize != 6 ) // TODO: What is this buffer? Not faces. thunderhawk_body.tpl
              continue;
            AddFaces( buffer, meshBuffer );
            break;
          case GeometryElementType.Interleaved:
            AddInterleavedData( buffer, meshBuffer );
            break;
          //case GeometryElementType.BoneId:
          //  AddSkinCompoundBoneIds( buffer, meshBuffer );
          //  break;
        }
      }

      ApplySkinCompoundData();
      AddMaterial();

      return Mesh;
    }

    public void ParentMeshToBone( objOBJ boneObject )
    {
      if ( boneObject.GetName() is null )
        return;

      for ( var i = 0; i < Mesh.VertexCount; i++ )
        AddVertexWeight( boneObject.id, 1, i );
    }

    #endregion

    #region Face Methods

    private void AddFaces( GeometryBuffer buffer, GeometryMeshBufferInfo bufferInfo )
    {
      Reader.Position = buffer.StartOffset;
      var offset = Submesh.BufferInfo.FaceOffset;
      var startIndex = offset + bufferInfo.SubBufferOffset / buffer.ElementSize;
      var endIndex = startIndex + Submesh.BufferInfo.FaceCount;

      var faceSerializer = new FaceSerializer( buffer );
      foreach ( var face in faceSerializer.DeserializeRange( Reader, startIndex, endIndex ) )
      {
        var assimpFace = new Assimp.Face();
        assimpFace.Indices.Add( VertexLookup[ face.A ] );
        assimpFace.Indices.Add( VertexLookup[ face.B ] );
        assimpFace.Indices.Add( VertexLookup[ face.C ] );

        Mesh.Faces.Add( assimpFace );
      }
    }

    #endregion

    #region Vertex Methods

    private void AddVertices( GeometryBuffer buffer, GeometryMeshBufferInfo bufferInfo )
    {
      Reader.Position = buffer.StartOffset;
      var offset = Submesh.BufferInfo.VertexOffset;
      var startIndex = offset + bufferInfo.SubBufferOffset / buffer.ElementSize;
      var endIndex = startIndex + Submesh.BufferInfo.VertexCount;

      var scale = new Vector3D( 1, 1, 1 );
      if ( Submesh.Scale.HasValue )
        scale = Submesh.Scale.Value.ToAssimp();

      var pos = new Vector3D( 0, 0, 0 );
      if ( Submesh.Position.HasValue )
        pos = Submesh.Position.Value.ToAssimp();

      var vertexSerializer = new VertexSerializer( buffer );
      foreach ( var vertex in vertexSerializer.DeserializeRange( Reader, startIndex, endIndex ) )
      {
        Mesh.Vertices.Add( vertex.Position.ToAssimpVector3D() * scale + pos );

        if ( vertex.HasNormal )
          Mesh.Normals.Add( vertex.Normal.ToAssimpVector3D() );

        if ( vertex.HasSkinningData )
          AddVertexSkinningData( vertex );

        VertexLookup.Add( offset++, VertexLookup.Count );
      }
    }

    private void AddVertexSkinningData( Vertex vertex )
    {
      var boneIds = Submesh.BoneIds;
      var set = new HashSet<short>();

      if ( vertex.HasWeight1)
        AddVertexWeight( vertex.Index1, vertex.Weight1 );
      if ( vertex.HasWeight2)
        AddVertexWeight( vertex.Index2, vertex.Weight2 );
      if ( vertex.HasWeight3)
        AddVertexWeight( vertex.Index3, vertex.Weight3 );
      //if ( vertex.Weight4.HasValue)
      //  AddVertexWeight( vertex.Index4, vertex.Weight4.Value );
    }

    private void AddVertexWeight( short boneObjectId, float weight, int vertIndex = -1 )
    {
      if ( boneObjectId == -1 )
        return;

      if ( vertIndex == -1 )
        vertIndex = Mesh.VertexCount - 1;

      var bone = GetOrCreateBone( boneObjectId );
      bone.VertexWeights.Add( new VertexWeight( vertIndex, weight ) );
    }

    #endregion

    #region Interleaved Methods

    private void AddInterleavedData( GeometryBuffer buffer, GeometryMeshBufferInfo bufferInfo )
    {
      Reader.Position = buffer.StartOffset;
      var offset = Submesh.BufferInfo.VertexOffset;
      var startIndex = offset + ( bufferInfo.SubBufferOffset / buffer.ElementSize );
      var endIndex = startIndex + Submesh.BufferInfo.VertexCount;

      var interleavedSerializer = new InterleavedDataSerializer( buffer );
      foreach ( var datum in interleavedSerializer.DeserializeRange( Reader, startIndex, endIndex ) )
      {
        if ( datum.HasUV0 ) AddVertexUV( 0, datum.UV0 );
        if ( datum.HasUV1 ) AddVertexUV( 1, datum.UV1 );
        if ( datum.HasUV2 ) AddVertexUV( 2, datum.UV2 );
        if ( datum.HasUV3 ) AddVertexUV( 3, datum.UV3 );
        if ( datum.HasUV4 ) AddVertexUV( 4, datum.UV4 );

        // TODO: Assimp only allows 1 tangent channel.
        if ( datum.HasTangent0 ) AddVertexTangent( 0, datum.Tangent0 );

        if ( datum.HasColor0 ) AddVertexColor( 0, datum.Color0 );
        if ( datum.HasColor1 ) AddVertexColor( 1, datum.Color1 );
        if ( datum.HasColor2 ) AddVertexColor( 2, datum.Color2 );
      }
    }

    private void AddVertexTangent( byte channel, Vector4 tangentVector )
    {
      Mesh.Tangents.Add( tangentVector.ToAssimpVector3D() );
    }

    private void AddVertexUV( byte channel, Vector4 uvVector )
    {
      if ( !Submesh.UvScaling.TryGetValue( channel, out var scaleFactor ) )
        scaleFactor = 1;

      var scaleVector = new Vector3D( scaleFactor );
      var scaledUvVector = uvVector.ToAssimpVector3D() * scaleVector;

      Mesh.TextureCoordinateChannels[ channel ].Add( scaledUvVector );
      Mesh.UVComponentCount[ channel ] = 2;
    }

    private void AddVertexColor( byte channel, Vector4 colorVector )
    {
      var color = colorVector.ToAssimpColor4D();
      Mesh.VertexColorChannels[ channel ].Add( color );
    }

    #endregion

    #region Skin Compound Methods

    private void AddSkinCompoundBoneIds( GeometryBuffer buffer, GeometryMeshBufferInfo bufferInfo )
    {
      var offset = Submesh.BufferInfo.VertexOffset;
      var startIndex = offset + bufferInfo.SubBufferOffset / buffer.ElementSize;
      var endIndex = startIndex + Submesh.BufferInfo.VertexCount;

      var boneIds = Submesh.BoneIds;

      Reader.Position = buffer.StartOffset + bufferInfo.SubBufferOffset + offset * buffer.ElementSize;
      for ( var i = startIndex; i < endIndex; i++ )
      {
        var boneIndex = boneIds[ Reader.ReadInt32() ];
        var vertIndex = VertexLookup[ offset++ ];
        AddVertexWeight( boneIndex, 1, vertIndex );
      }
    }

    private void ApplySkinCompoundData()
    {
      if ( SkinCompoundId == -1 )
        return;

      if ( !Context.SkinCompounds.TryGetValue( SkinCompoundId, out var skinCompound ) )
        return;

      var offset = Submesh.BufferInfo.VertexOffset;
      var vertCount = Submesh.BufferInfo.VertexCount;
      var skinCompoundVertOffset = skinCompound.VertexLookup.Min( x => x.Key );

      var boneLookup = new Dictionary<short, short>();
      foreach ( var bonePair in skinCompound.Bones )
      {
        var boneId = bonePair.Key;
        var sourceBone = bonePair.Value;

        if ( !boneLookup.TryGetValue( boneId, out var adjustedBoneId ) )
        {
          var boneObject = Graph.objects[ boneId ];
          var boneName = Object.GetName();
          if ( boneName is null )
            adjustedBoneId = boneId;
          else if ( boneObject.GetName() != boneName )
          {
            var parentBoneObject = Graph.objects.FirstOrDefault( x => x.GetName() == boneName );
            if ( parentBoneObject != null )
            {
              boneObject = parentBoneObject;
              adjustedBoneId = boneObject.id;
            }
          }

          boneLookup.Add( boneId, adjustedBoneId );
        }

        foreach ( var weight in sourceBone.VertexWeights )
        {
          var trueVertOffset = weight.VertexID + skinCompoundVertOffset;
          if ( !VertexLookup.TryGetValue( trueVertOffset, out var translatedVertOffset ) )
            continue;

          var skinVertex = skinCompound.Mesh.Vertices[ weight.VertexID ];
          var targetVertex = Mesh.Vertices[ translatedVertOffset ];
          Debug.Assert( skinVertex.X == targetVertex.X );
          Debug.Assert( skinVertex.Y == targetVertex.Y );
          Debug.Assert( skinVertex.Z == targetVertex.Z );

          AddVertexWeight( adjustedBoneId, 1, translatedVertOffset );
        }
      }
    }

    #endregion

    #region Bone Methods

    private Bone GetOrCreateBone( short boneObjectId )
    {
      var boneObject = Graph.objects[ boneObjectId ];
      var boneName = boneObject.GetName();
      if ( boneName is null )
        boneName = $"Bone{boneObjectId}";

      if ( !BoneNames.TryGetValue( boneName, out var bone ) )
      {
        System.Numerics.Matrix4x4.Invert( boneObject.MatrixLT, out var invMatrix );
        var transform = invMatrix.ToAssimp();
        transform.Transpose();

        bone = new Bone
        {
          Name = boneName,
          OffsetMatrix = transform
        };

        Mesh.Bones.Add( bone );
        Bones.Add( boneObjectId, bone );
        BoneNames.Add( boneName, bone );
      }

      return bone;
    }

    private void AddMaterial()
    {
      var submeshMaterial = Submesh.Material;
      if ( submeshMaterial is null )
        return;

      var materialName = submeshMaterial.ShadingMaterialMaterial;
      var textureName = submeshMaterial.ShadingMaterialTexture;
      var exportMatName = submeshMaterial.MaterialName;

      if ( Context.MaterialIndices.TryGetValue( exportMatName, out var materialIndex ) )
      {
        Mesh.MaterialIndex = materialIndex;
        return;
      }

      var material = new Material { Name = exportMatName };
      material.TextureDiffuse = new TextureSlot( textureName, TextureType.Diffuse, 0, TextureMapping.FromUV, 0, blendFactor: 0, TextureOperation.Add, TextureWrapMode.Wrap, TextureWrapMode.Wrap, 0 ); ;
      material.TextureNormal = new TextureSlot( $"{textureName}_nm", TextureType.Normals, 1, TextureMapping.FromUV, 0, blendFactor: 0, TextureOperation.Add, TextureWrapMode.Wrap, TextureWrapMode.Wrap, 0 );
      material.TextureSpecular = new TextureSlot( $"{textureName}_spec", TextureType.Specular, 2, TextureMapping.FromUV, 0, blendFactor: 0, TextureOperation.Add, TextureWrapMode.Wrap, TextureWrapMode.Wrap, 0 );

      materialIndex = Context.Scene.Materials.Count;

      Context.Scene.Materials.Add( material );
      Context.MaterialIndices.Add( exportMatName, materialIndex );
      Mesh.MaterialIndex = materialIndex;
    }

    #endregion

  }

}
