using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderGlossiness
  {

    [ScriptingProperty( "bias" )]
    public Single Bias { get; set; }

    [ScriptingProperty( "scale" )]
    public Single Scale { get; set; }

  }

}
