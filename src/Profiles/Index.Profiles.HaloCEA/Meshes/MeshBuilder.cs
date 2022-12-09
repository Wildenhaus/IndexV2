using System.Runtime.InteropServices;
using Assimp;
using Index.Profiles.HaloCEA.Common;
using LibSaber.HaloCEA.Structures;
using LibSaber.Shared.Structures;

namespace Index.Profiles.HaloCEA.Meshes
{

  public class MeshBuilder
  {

    #region Data Members

    private readonly Dictionary<int, int> _vertexLookup;

    #endregion

    #region Properties

    public Mesh Mesh { get; }
    public Dictionary<int, Bone> Bones { get; }

    protected SceneContext Context { get; }

    protected SaberObject Object { get; }
    protected SaberObject SkinCompoundObject { get; }
    protected SaberObject SharingObject { get; }

    protected SubmeshInfo SubmeshInfo { get; }

    #endregion

    #region Constructor

    private MeshBuilder( SceneContext context, SaberObject obj, SubmeshInfo submeshInfo )
    {
      Context = context;
      Object = obj;
      SubmeshInfo = submeshInfo;

      if ( submeshInfo.SkinCompoundId.HasValue )
        SkinCompoundObject = Context.Objects[ submeshInfo.SkinCompoundId.Value ];

      if ( obj.SharingObjectInfo is not null )
        SharingObject = Context.Objects[ obj.SharingObjectInfo.SharingObjectId ];

      Mesh = new Mesh( obj.ObjectInfo.Name, PrimitiveType.Triangle );
      Bones = new Dictionary<int, Bone>();

      _vertexLookup = new Dictionary<int, int>();
    }

    public static Mesh Build( SceneContext context, SaberObject obj, SubmeshInfo submeshInfo )
    {
      var builder = new MeshBuilder( context, obj, submeshInfo );
      builder.Build();

      var vtxCount = builder.Mesh.VertexCount;
      foreach ( var face in builder.Mesh.Faces )
      {
        if ( face.Indices.Any( x => x >= vtxCount ) )
          System.Diagnostics.Debugger.Break();
      }

      return builder.Mesh;
    }

    #endregion

    #region Public Methods

    public void Build()
    {
      AddVertices();
      AddFaces();
      AddInterleavedData();
      AddMaterials();
      AddSkinning();
    }

    #endregion

    #region Private Methods

    #region Vertices

    private void AddVertices()
    {
      if ( SkinCompoundObject is not null )
        AddSkinCompoundVertices();
      else if ( SharingObject is not null )
        AddSharingObjectVertices();
      else
        AddSubmeshVertices();
    }

    private void AddSkinCompoundVertices()
    {
      var mesh = Mesh;
      var skinData = SkinCompoundObject.SkinningData;
      var skinObjectId = Object.ObjectInfo.Id - skinData.FirstObjectId;

      var vertices = CollectionsMarshal.AsSpan( SkinCompoundObject.Vertices );
      for ( var i = 0; i < vertices.Length; i++ )
      {
        var skinObjectIds = skinData.BoneIds[ i ];

        if ( skinObjectIds.X != skinObjectId )
          continue;

        _vertexLookup.Add( i, mesh.Vertices.Count );

        var vertex = vertices[ i ];
        mesh.Vertices.Add( vertex.Position.ToVector3D() );
        mesh.Normals.Add( vertex.Normal.ToVector3D() );
      }
    }

    private void AddSharingObjectVertices()
    {
      var mesh = Mesh;
      var sharingInfo = Object.SharingObjectInfo;
      var vertexOffset = sharingInfo.VertexOffset;
      var vertexCount = SubmeshInfo.VertexInfo.Value.VertexCount;

      var vtxScale = new Vector3<short>( 1, 1, 1 );
      var vtxPos = new Vector3<short>( 0, 0, 0 );
      if ( Object.TranslationVectors is not null )
      {
        vtxScale = Object.TranslationVectors.Scale;
        vtxPos = Object.TranslationVectors.Translation;
      }

      var vertices = CollectionsMarshal.AsSpan( SharingObject.Vertices ).Slice( vertexOffset, vertexCount );
      foreach ( var vertex in vertices )
      {
        mesh.Vertices.Add( new Vector3D(
          ( vertex.Position.X * vtxScale.X ) + vtxPos.X,
          ( vertex.Position.Y * vtxScale.Y ) + vtxPos.Y,
          ( vertex.Position.Z * vtxScale.Z ) + vtxPos.Z ) );

        mesh.Normals.Add( vertex.Normal.ToVector3D() );
      }
    }

    private void AddSubmeshVertices()
    {
      var mesh = Mesh;
      var vertexOffset = SubmeshInfo.VertexInfo.Value.VertexOffset;
      var vertexCount = SubmeshInfo.VertexInfo.Value.VertexCount;

      var vertices = CollectionsMarshal.AsSpan( Object.Vertices ).Slice( vertexOffset, vertexCount );
      foreach ( var vertex in vertices )
      {
        mesh.Vertices.Add( vertex.Position.ToVector3D() );
        mesh.Normals.Add( vertex.Normal.ToVector3D() );
      }
    }

    #endregion

    #region Faces

    private void AddFaces()
    {
      if ( SkinCompoundObject is not null )
        AddSkinCompoundFaces();
      else if ( SharingObject is not null )
        AddSharingObjectFaces();
      else
        AddSubmeshFaces();
    }

    private void AddSkinCompoundFaces()
    {
      var mesh = Mesh;
      var skinData = SkinCompoundObject.SkinningData;
      var skinObjectId = Object.ObjectInfo.Id - skinData.FirstObjectId;
      var vertexLookup = _vertexLookup;

      var faces = CollectionsMarshal.AsSpan( SkinCompoundObject.Faces );
      for ( var i = 0; i < faces.Length; i++ )
      {
        var face = faces[ i ];

        if ( !vertexLookup.TryGetValue( face.A, out var a ) ) continue;
        if ( !vertexLookup.TryGetValue( face.B, out var b ) ) continue;
        if ( !vertexLookup.TryGetValue( face.C, out var c ) ) continue;

        mesh.Faces.Add( new Assimp.Face( new int[] { a, b, c } ) );
      }
    }

    private void AddSharingObjectFaces()
    {
      var mesh = Mesh;
      var sharingInfo = Object.SharingObjectInfo;
      var faceOffset = sharingInfo.FaceOffset;
      var faceCount = SubmeshInfo.FaceInfo.Value.FaceCount;

      var faces = CollectionsMarshal.AsSpan( SharingObject.Faces ).Slice( faceOffset, faceCount );
      foreach ( var face in faces )
        mesh.Faces.Add( new Assimp.Face( new int[] { face.A, face.B, face.C } ) );
    }

    private void AddSubmeshFaces()
    {
      var mesh = Mesh;
      var vertexOffset = SubmeshInfo.VertexInfo.Value.VertexOffset;
      var faceOffset = SubmeshInfo.FaceInfo.Value.FaceOffset;
      var faceCount = SubmeshInfo.FaceInfo.Value.FaceCount;

      var faces = CollectionsMarshal.AsSpan( Object.Faces ).Slice( faceOffset, faceCount );
      foreach ( var face in faces )
      {
        var a = face.A - vertexOffset;
        var b = face.B - vertexOffset;
        var c = face.C - vertexOffset;

        mesh.Faces.Add( new Assimp.Face( new int[] { a, b, c } ) );
      }
    }

    #endregion

    #region Interleaved Data

    private void AddInterleavedData()
    {
      if ( SkinCompoundObject is not null )
        AddSkinCompoundInterleavedData();
      else if ( SharingObject is not null )
        AddSharingObjectInterleavedData();
      else
        AddSubmeshInterleavedData();
    }

    private void AddSkinCompoundInterleavedData()
    {
      var skinData = SkinCompoundObject.SkinningData;
      var skinObjectId = Object.ObjectInfo.Id - skinData.FirstObjectId;

      var interleavedData = SkinCompoundObject.InterleavedDataBuffer.ElementData.AsSpan();
      for ( var i = 0; i < interleavedData.Length; i++ )
      {
        var skinObjectIds = skinData.BoneIds[ i ];

        if ( skinObjectIds.X != skinObjectId )
          continue;

        AddInterleavedDatum( interleavedData[ i ], SkinCompoundObject.UvScaling );
      }
    }

    private void AddSharingObjectInterleavedData()
    {
      var sharingInfo = Object.SharingObjectInfo;
      var vertexOffset = sharingInfo.VertexOffset;
      var vertexCount = SubmeshInfo.VertexInfo.Value.VertexCount;

      if ( Object.InterleavedDataBuffer is null )
        return;

      var interleavedData = SharingObject.InterleavedDataBuffer.ElementData.AsSpan( vertexOffset, vertexCount );
      foreach ( var datum in interleavedData )
        AddInterleavedDatum( datum, Object.UvScaling );
    }

    private void AddSubmeshInterleavedData()
    {
      if ( Object.InterleavedDataBuffer is null )
        return;

      var vertexOffset = SubmeshInfo.VertexInfo.Value.VertexOffset;
      var vertexCount = SubmeshInfo.VertexInfo.Value.VertexCount;

      var interleavedData = Object.InterleavedDataBuffer.ElementData.AsSpan( vertexOffset, vertexCount );
      foreach ( var datum in interleavedData )
        AddInterleavedDatum( datum, Object.UvScaling );
    }

    private void AddInterleavedDatum( InterleavedData datum, UvScalingMap uvScaling )
    {
      // UVs/Texture Coordinates
      if ( datum.UV0.HasValue ) AddVertexUV( datum.UV0, 0, uvScaling );
      if ( datum.UV1.HasValue ) AddVertexUV( datum.UV1, 1, uvScaling );
      if ( datum.UV2.HasValue ) AddVertexUV( datum.UV2, 2, uvScaling );
      if ( datum.UV3.HasValue ) AddVertexUV( datum.UV3, 3, uvScaling );

      // Vertex Colors
      if ( datum.Color0.HasValue ) AddVertexColor( datum.Color0, 0 );
      if ( datum.Color1.HasValue ) AddVertexColor( datum.Color1, 1 );
      if ( datum.Color2.HasValue ) AddVertexColor( datum.Color2, 2 );

      // Tangents
      // TODO: Assimp only supports a single tangent channel?
      if ( datum.Tangent0.HasValue ) AddVertexTangent( datum.Tangent0, 0 );
    }

    private void AddVertexUV( Vector3<float>? coords, int channel, UvScalingMap uvScaling )
    {
      var u = coords.Value.X;
      var v = coords.Value.Y;

      if ( uvScaling is not null )
      {
        if ( uvScaling.TryGetValue( channel, out var scale ) )
        {
          u *= scale;
          v *= scale;
        }
      }

      Mesh.TextureCoordinateChannels[ channel ].Add( new Vector3D( u, v, 0 ) );
      Mesh.UVComponentCount[ channel ] = 2;
    }

    private void AddVertexColor( Vector4<byte>? color, int channel )
    {
      Mesh.VertexColorChannels[ channel ].Add( color.Value.ToColor4D() );
    }

    private void AddVertexTangent( Vector4<float>? tangent, int channel )
    {
      Mesh.Tangents.Add( tangent.Value.ToVector3D() );
    }

    #endregion

    #region Materials

    private void AddMaterials()
    {
      if ( SkinCompoundObject is not null )
        AddSkinCompoundMaterials();
      else if ( SharingObject is not null )
        AddSharingObjectMaterials();
      else
        AddSubmeshMaterials();
    }

    private void AddSkinCompoundMaterials()
    {
      var skinData = SkinCompoundObject.SkinningData;
      var skinObjectId = Object.ObjectInfo.Id - skinData.FirstObjectId;

      var materialList = SubmeshInfo.MaterialList;
      var materialInfo = materialList[ 0 ];
      ASSERT( materialInfo.Sentinel_010E.HasValue );

      var materialIndex = materialInfo.Sentinel_010E.Value.UnkMaterialIndex;
      if ( materialIndex == -1 )
        return;

      Mesh.MaterialIndex = materialIndex;
    }

    private void AddSharingObjectMaterials()
    {
      var materialList = SubmeshInfo.MaterialList;
      var materialInfo = materialList[ 0 ];
      ASSERT( materialInfo.Sentinel_010E.HasValue );

      var materialIndex = materialInfo.Sentinel_010E.Value.UnkMaterialIndex;
      if ( materialIndex == -1 )
        return;

      Mesh.MaterialIndex = materialIndex;
    }

    private void AddSubmeshMaterials()
    {
      var materialList = SubmeshInfo.MaterialList;
      //ASSERT( materialList.Count == 1 );

      var materialInfo = materialList[ 0 ];
      ASSERT( materialInfo.Sentinel_010E.HasValue );

      var materialIndex = materialInfo.Sentinel_010E.Value.UnkMaterialIndex;
      if ( materialIndex == -1 )
        return;

      Mesh.MaterialIndex = materialIndex;
    }

    #endregion

    #region Skinning

    private void AddSkinning()
    {
      if ( Object.SkinningData is null )
        return;

      AddSubmeshSkinning();
    }

    private void AddSubmeshSkinning()
    {
      var mesh = Mesh;

      var vertexOffset = SubmeshInfo.VertexInfo.Value.VertexOffset;
      var vertexCount = SubmeshInfo.VertexInfo.Value.VertexCount;

      var skinData = Object.SkinningData;
      var rootBoneObjectId = skinData.FirstObjectId;

      var boneIdBuffer = skinData.BoneIds.AsSpan( vertexOffset, vertexCount );
      var boneWeightBuffer = skinData.BoneIds.AsSpan( vertexOffset, vertexCount );
      for ( var i = 0; i < vertexCount; i++ )
      {
        var boneIds = boneIdBuffer[ i ];
        var boneWeights = boneWeightBuffer[ i ];

        var set = new HashSet<byte>();
        if ( boneWeights.X > 0 && set.Add( boneIds.X ) ) AddVertexWeight( i, rootBoneObjectId, boneIds.X, boneWeights.X );
        if ( boneWeights.Y > 0 && set.Add( boneIds.Y ) ) AddVertexWeight( i, rootBoneObjectId, boneIds.Y, boneWeights.Y );
        if ( boneWeights.Z > 0 && set.Add( boneIds.Z ) ) AddVertexWeight( i, rootBoneObjectId, boneIds.Z, boneWeights.Z );
        if ( boneWeights.W > 0 && set.Add( boneIds.W ) ) AddVertexWeight( i, rootBoneObjectId, boneIds.W, boneWeights.W );
      }
    }

    private void AddVertexWeight( int vertIndex, short rootBoneObjectId, short boneOffset, float weight )
    {
      var boneObjectId = ( short ) ( rootBoneObjectId + boneOffset );
      var boneObject = Context.Objects[ boneObjectId ];
      var boneId = boneObject.BoneId;
      //ASSERT( boneId != -1 );
      if ( boneId == -1 ) // TODO
        return;

      var bone = GetOrCreateBone( boneId );
      bone.VertexWeights.Add( new VertexWeight( vertIndex, weight ) );
    }

    private Bone GetOrCreateBone( int boneId )
    {
      if ( Bones.TryGetValue( boneId, out var bone ) )
        return bone;

      var boneObject = Context.BoneObjects[ boneId ];
      bone = new Bone { Name = boneObject.ObjectInfo.Name };

      var matrix = boneObject.Matrix.Value.ToMatrix4();
      matrix.Transpose();
      bone.OffsetMatrix = matrix;

      Bones.Add( boneId, bone );
      Mesh.Bones.Add( bone );

      return bone;
    }

    #endregion

    #endregion

  }

}
