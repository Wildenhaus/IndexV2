using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderParallax
  {

    [ScriptingProperty( "scale" )]
    public Single Scale { get; set; }

    [ScriptingProperty( "shadow" )]
    public Boolean Shadow { get; set; }

  }

}
