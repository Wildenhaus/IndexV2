using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderAdvanced
  {

    [ScriptingProperty( "causticsMultiplier" )]
    public Single CausticsMultiplier { get; set; }

    [ScriptingProperty( "fresnel" )]
    public ShaderFresnel Fresnel { get; set; }

    [ScriptingProperty( "kSoft" )]
    public Single KSoft { get; set; }

  }

}
