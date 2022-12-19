using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderReflection
  {

    [ScriptingProperty( "contrast" )]
    public Single Contrast { get; set; }

    [ScriptingProperty( "disturbance" )]
    public Single Disturbance { get; set; }

    [ScriptingProperty( "frameSkip" )]
    public Int32 FrameSkip { get; set; }

    [ScriptingProperty( "intensity" )]
    public Single Intensity { get; set; }

    [ScriptingProperty( "mipPower" )]
    public Single MipPower { get; set; }

    [ScriptingProperty( "useCubeMap" )]
    public Boolean UseCubeMap { get; set; }

  }

}
