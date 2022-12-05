using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibSaber.HaloCEA.Enumerations
{

  public enum CEAFileType : int
  {

    // TODO: Document
    SceneData = 0,

    // TODO: Document
    Data = 1,

    /// <summary>
    ///   A file the stores scripts, settings, and material properties.
    /// </summary>
    CacheBlock = 2,

    // TODO: Document
    Shader = 3,

    // TODO: Document
    ShaderCache = 4,

    // TODO: Document
    TextureInfo = 5,

    // TODO: Document
    Texture = 6,

    // TODO: Document
    TextureMips64 = 7,

    // TODO: Document
    SoundData = 8,

    [Obsolete( "Unused" )]
    Sound = 9,

    // TODO: Document
    WaveBanks_mem = 10,

    // TODO: Document
    WaveBanks_strm_file = 11,

    /// <summary>
    ///   A file containing 3D mesh data for an in-game model.
    /// </summary>
    Template = 12,

    [Obsolete( "Unused" )]
    VoiceSpline = 13,

    // TODO: Document
    String = 14,

    [Obsolete( "Unused" )]
    Ragdoll = 15,

    /// <summary>
    ///   A file containing 3D mesh data and other properties of an in-game level.
    /// </summary>
    Scene = 16,

    // TODO: Document
    HavokHkx = 17,

    // TODO: Document
    FlashGfx = 18,

    // TODO: Document
    TextureDistanceFile = 19,

    [Obsolete( "Unused" )]
    CheckPointTexFile = 20,

    [Obsolete( "Unused" )]
    LoadingScreenGfx = 21,

    // TODO: Document
    SceneGrs = 22,

    [Obsolete( "Unused" )]
    SceneScr = 23,

    [Obsolete( "Unused" )]
    SceneAnimBin = 24,

    // TODO: Document
    SceneRain = 25,

    // TODO: Document
    SceneCDT = 26,

    // TODO: Document
    // ShadowMap?
    SceneSM = 27,

    [Obsolete( "Unused" )]
    SceneSLO = 28,

    // TODO: Document
    SceneVis = 29,

    // TODO: Document
    AnimStream = 30,

    [Obsolete( "Unused" )]
    AnimBank = 31

  }

}
