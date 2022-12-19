using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderSoftFresnel
  {

    [ScriptingProperty( "edge" )]
    public Boolean Edge { get; set; }

    [ScriptingProperty( "edgeHighlightIntensity" )]
    public Single EdgeHighlightIntensity { get; set; }

    [ScriptingProperty( "enabled" )]
    public Boolean Enabled { get; set; }

    [ScriptingProperty( "power" )]
    public Single Power { get; set; }

  }

}
