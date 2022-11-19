namespace Index.Profiles.HaloCEA.FileSystem
{

  public enum CEAFileType : int
  {
    SceneData = 0,
    Data = 1,
    CacheBlock = 2,
    Shader = 3,
    ShaderCache = 4,
    TextureInfo = 5,
    Texture = 6,
    TextureMips64 = 7,
    SoundData = 8,
    [Obsolete( "Unused" )]
    Sound = 9,
    WaveBanks_mem = 10,
    WaveBanks_strm_file = 11,
    Template = 12,
    [Obsolete( "Unused" )]
    VoiceSpline = 13,
    String = 14,
    [Obsolete( "Unused" )]
    Ragdoll = 15,
    Scene = 16,
    HavokHkx = 17,
    FlashGfx = 18,
    TextureDistanceFile = 19,
    [Obsolete( "Unused" )]
    CheckPointTexFile = 20,
    [Obsolete( "Unused" )]
    LoadingScreenGfx = 21,
    SceneGrs = 22,
    [Obsolete( "Unused" )]
    SceneScr = 23,
    [Obsolete( "Unused" )]
    SceneAnimBin = 24,
    SceneRain = 25,
    SceneCDT = 26,
    SceneSM = 27,
    [Obsolete( "Unused" )]
    SceneSLO = 28,
    SceneVis = 29,
    AnimStream = 30,
    [Obsolete( "Unused" )]
    AnimBank = 31

  }

}
