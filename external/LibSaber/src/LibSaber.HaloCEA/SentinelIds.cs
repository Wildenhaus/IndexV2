namespace LibSaber.HaloCEA
{

  public static class SentinelIds
  {
    public const SentinelId Delimiter = 0x0001;

    public const SentinelId Sentinel_0102 = 0x0102;
    public const SentinelId Sentinel_0103 = 0x0103; // unk submesh data
    public const SentinelId Sentinel_0104 = 0x0104; // submesh data
    public const SentinelId Sentinel_0105 = 0x0105; // submesh face data

    public const SentinelId ObjectSubmeshData = 0x0107; // submesh list/container
    public const SentinelId Sentinel_0108 = 0x0108;
    public const SentinelId Sentinel_0109 = 0x0109;
    public const SentinelId Sentinel_010A = 0x010A;
    public const SentinelId Sentinel_010B = 0x010B; // submesh material info
    public const SentinelId Sentinel_010C = 0x010C; // reads 4 bytes but doesn't seem to be assigned anywhere
    public const SentinelId Sentinel_010D = 0x010D; // submesh vertex data
    public const SentinelId Sentinel_010E = 0x010E; // submesh material info

    public const SentinelId Sentinel_0110 = 0x0110;

    public const SentinelId Sentinel_0114 = 0x0114; // unk material info
    public const SentinelId ObjectAffixes = 0x0115; // unk affixes
    public const SentinelId ObjectSkinningData = 0x0116; // unk vertex bind container
    public const SentinelId Sentinel_0117 = 0x0117; // unk vertex bind data
    public const SentinelId Sentinel_0118 = 0x0118;
    public const SentinelId Sentinel_0119 = 0x0119;
    public const SentinelId ObjectSkinningBoneWeights = 0x011A; // unk vertex blend weight
    public const SentinelId Sentinel_011B = 0x011B;
    public const SentinelId Sentinel_011C = 0x011C; // unk submesh data
    public const SentinelId ObjectBoundingBox = 0x011D; // mesh bounding box
    public const SentinelId Sentinel_011E = 0x011E;
    public const SentinelId Sentinel_011F = 0x011F; // unk material data
    public const SentinelId Sentinel_0120 = 0x0120; // unk submesh data
    public const SentinelId Sentinel_0121 = 0x0121;
    public const SentinelId Sentinel_0122 = 0x0122;
    public const SentinelId Sentinel_0123 = 0x0123;
    public const SentinelId Sentinel_0124 = 0x0124;
    public const SentinelId Sentinel_0125 = 0x0125;
    public const SentinelId Sentinel_0126 = 0x0126;
    public const SentinelId Sentinel_0127 = 0x0127;
    public const SentinelId Sentinel_0128 = 0x0128; // unk submesh data, same as 0138 but with two less fields
    public const SentinelId ObjectSharingObjectInfo = 0x0129; // SharingObjectInfo
    public const SentinelId Sentinel_012A = 0x012A;
    public const SentinelId ObjectParentId = 0x012B; // parent id
    public const SentinelId Sentinel_012C = 0x012C; // object count
    public const SentinelId Sentinel_012E = 0x012E; // geometry flags
    public const SentinelId ObjectUvScaling = 0x012F; // unk mesh material data
    public const SentinelId ObjectInterleavedBuffer = 0x0130; // Interleaved Data Buffer (uv, tan, etc)
    public const SentinelId Sentinel_0131 = 0x0131;
    public const SentinelId Sentinel_0132 = 0x0132; // unk submesh transform?
    public const SentinelId ObjectSkinningBoneIds = 0x0133; // unk vertex blend indices?
    public const SentinelId Sentinel_0134 = 0x0134; // skincompound id
    public const SentinelId Sentinel_0135 = 0x0135; // PosScaleTransformVectors
    public const SentinelId Sentinel_0136 = 0x0136;
    public const SentinelId Sentinel_0137 = 0x0137;
    public const SentinelId Sentinel_0138 = 0x0138;

    public const SentinelId Sentinel_0154 = 0x0154;
    public const SentinelId TextureList = 0x0155; // texture list
    public const SentinelId TextureListEntry = 0x0156; // texture list entry

    public const SentinelId SceneProps = 0x01B8; // ScenePropList
    public const SentinelId Sentinel_01B9 = 0x01B9; // SceneProp
    public const SentinelId Sentinel_01BA = 0x01BA; // scripting?
    public const SentinelId Sentinel_01BB = 0x01BB;
    public const SentinelId Sentinel_01BC = 0x01BC;
    public const SentinelId Sentinel_01BD = 0x01BD;

    public const SentinelId Sentinel_01E0 = 0x01E0;
    public const SentinelId Sentinel_01E1 = 0x01E1;
    public const SentinelId Sentinel_01E2 = 0x01E2;

    public const SentinelId Sentinel_01EA = 0x01EA;

    public const SentinelId Sentinel_021D = 0x021D; // always 0?
    public const SentinelId Sentinel_021E = 0x021E;
    public const SentinelId Sentinel_021F = 0x021F;
    public const SentinelId Sentinel_0220 = 0x0220; // Some kind of 
    public const SentinelId Sentinel_0221 = 0x0221;
    public const SentinelId Sentinel_0222 = 0x0222;

    public const SentinelId SceneLights = 0x0280; // Scene Lights List

    public const SentinelId Sentinel_0282 = 0x0282;
    public const SentinelId Sentinel_0283 = 0x0283;
    public const SentinelId Sentinel_0284 = 0x0284;
    public const SentinelId Sentinel_0285 = 0x0285;
    public const SentinelId Sentinel_0286 = 0x0286;

    public const SentinelId Sentinel_0288 = 0x0288;
    public const SentinelId Sentinel_0289 = 0x0289;
    public const SentinelId Sentinel_028A = 0x028A;
    public const SentinelId Sentinel_028B = 0x028B;
    public const SentinelId Sentinel_028C = 0x028C;

    public const SentinelId Sentinel_02E4 = 0x02E4; // template
    public const SentinelId Sentinel_02E5 = 0x02E5; // template header
    public const SentinelId Sentinel_02E6 = 0x02E6; // animation sequence list

    public const SentinelId Sentinel_02E8 = 0x02E8; // object animation list
    public const SentinelId ObjectAnim = 0x02E9; // object animation
    public const SentinelId ObjectAnim_PTranslation = 0x02EA; // object animation translation spline
    public const SentinelId ObjectAnim_PRotation = 0x02EB; // object animation rotation spline
    public const SentinelId ObjectAnim_PScale = 0x02EC; // object animation scale spline
    public const SentinelId ObjectAnim_PVisibility = 0x02ED; // object animation visibility spline

    public const SentinelId AnimationSequence_TimeSec = 0x02F3; // AnimSeq.TimeSec?
    public const SentinelId AnimationSequence_LenFrame = 0x02F4; // AnimSeq.LenFrame?

    public const SentinelId ObjectAnim_IniTranslation = 0x02FA;  // object animation initial translation
    public const SentinelId ObjectAnim_IniRotation = 0x02FB; // object animation initial rotation
    public const SentinelId ObjectAnim_IniScale = 0x02FC; // object animation initial scale
    public const SentinelId AnimationSequence = 0x02FD; // AnimSeq
    public const SentinelId AnimationSequence_Name = 0x02FE; // AnimSeq.Name
    public const SentinelId AnimationSequence_StartFrame = 0x02FF; // AnimSeq.StartFrame
    public const SentinelId AnimationSequence_EndFrame = 0x0300; // AnimSeq.EndFrame
    public const SentinelId AnimationSequence_OffsetFrame = 0x0301; // AnimSeq.OffsetFrame

    public const SentinelId Sentinel_0304 = 0x0304; // export affixes?
    public const SentinelId Sentinel_0305 = 0x0305; // matrices. maybe MatrixLT for bones?
    public const SentinelId Sentinel_0306 = 0x0306;
    public const SentinelId AnimationSequence_BoundingBox = 0x0307; // AnimSeq.Bbox
    public const SentinelId Sentinel_0308 = 0x0308; // Object bounding box

    public const SentinelId ObjectAnim_IniVisibility = 0x030A; // object animation initial visibility

    public const SentinelId Sentinel_030C = 0x030C;
    public const SentinelId Sentinel_030D = 0x030D; // matrices
    public const SentinelId Sentinel_030E = 0x030E;
    public const SentinelId Sentinel_030F = 0x030F;

    public const SentinelId Sentinel_0311 = 0x0311; // LOD Definition List
    public const SentinelId Sentinel_0312 = 0x0312; // Template Info (scripting)
    public const SentinelId Sentinel_0313 = 0x0313;
    public const SentinelId AnimationSequence_ActionFrames = 0x0314; // AnimSeq.ActionFrames
    public const SentinelId Sentinel_0315 = 0x0315;

    public const SentinelId Sentinel_0316 = 0x0316; // Template Flags?

    public const SentinelId Sentinel_0348 = 0x0348;
    public const SentinelId Sentinel_0349 = 0x0349;
    public const SentinelId Sentinel_034A = 0x034A;

    public const SentinelId Sentinel_03AC = 0x03AC;
    public const SentinelId Sentinel_03AD = 0x03AD;
    public const SentinelId Sentinel_03AE = 0x03AE;
    public const SentinelId Sentinel_03AF = 0x03AF;
    public const SentinelId Sentinel_03B0 = 0x03B0;
    public const SentinelId Sentinel_03B1 = 0x03B1;
    public const SentinelId Sentinel_03B2 = 0x03B2;
    public const SentinelId Sentinel_03B3 = 0x03B3;
    public const SentinelId Sentinel_03B4 = 0x03B4;
    public const SentinelId Sentinel_03B5 = 0x03B5;

    public const SentinelId Sentinel_03B7 = 0x03B7;
    public const SentinelId Sentinel_03B8 = 0x03B8;

    public const SentinelId ObjectInfo = 0x03B9; // Object Info

    public const SentinelId Sentinel_03C0 = 0x03C0; // Scene Guid?

    public const SentinelId Sentinel_0424 = 0x0424;
    public const SentinelId Sentinel_0425 = 0x0425;

    public const SentinelId Sentinel_0482 = 0x0482;
    public const SentinelId Sentinel_0483 = 0x0483;
    public const SentinelId Sentinel_0484 = 0x0484;

  }

}
