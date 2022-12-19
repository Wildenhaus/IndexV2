using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderFresnel
  {

    [ScriptingProperty( "multiplier" )]
    public Single Multiplier { get; set; }

    [ScriptingProperty( "power" )]
    public Single Power { get; set; }

    [ScriptingProperty( "R0" )]
    public Single R0 { get; set; }

  }

}
