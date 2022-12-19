using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderFakeLight
  {

    [ScriptingProperty( "enabled" )]
    public Boolean Enabled { get; set; }

    [ScriptingProperty( "falloff_dist" )]
    public Single FalloffDistance { get; set; }

    [ScriptingProperty( "intensity" )]
    public Single Intensity { get; set; }

    [ScriptingProperty( "start_dist" )]
    public Single StartDistance { get; set; }

  }

}
