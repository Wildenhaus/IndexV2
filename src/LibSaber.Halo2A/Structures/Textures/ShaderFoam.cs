using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderFoam
  {

    [ScriptingProperty( "intensity" )]
    public Single Intensity { get; set; }

    [ScriptingProperty( "pos0" )]
    public Single Position0 { get; set; }

    [ScriptingProperty( "pos1" )]
    public Single Position1 { get; set; }

    [ScriptingProperty( "pos2" )]
    public Single Position2 { get; set; }

    [ScriptingProperty( "pos3" )]
    public Single Position3 { get; set; }

    [ScriptingProperty( "refluence" )]
    public ShaderFoamRefluence Refluence { get; set; }

    [ScriptingProperty( "shift" )]
    public Single Shift { get; set; }

    [ScriptingProperty( "shore" )]
    public ShaderFoamShore Shore { get; set; }

    [ScriptingProperty( "shoreFoamIntensity" )]
    public Single ShoreFoamIntensity { get; set; }

    [ScriptingProperty( "tiling" )]
    public Single Tiling { get; set; }

    [ScriptingProperty( "wave" )]
    public ShaderFoamWave Wave { get; set; }

    [ScriptingProperty( "waveFoamIntensity" )]
    public Single WaveFoamIntensity { get; set; }

  }

}
