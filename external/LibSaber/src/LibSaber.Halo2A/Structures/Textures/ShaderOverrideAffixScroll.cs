using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderOverrideAffixScroll
  {

    [ScriptingProperty( "override" )]
    public Boolean OverrideEnabled { get; set; }

    [ScriptingProperty( "speedX" )]
    public Single SpeedX { get; set; }

    [ScriptingProperty( "speedY" )]
    public Single SpeedY { get; set; }

  }

}
