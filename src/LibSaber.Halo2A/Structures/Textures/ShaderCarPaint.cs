using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderCarPaint
  {

    [ScriptingProperty( "glossiness" )]
    public ShaderGlossiness Glossiness { get; set; }

    [ScriptingProperty( "metalness" )]
    public SaberColor Metalness { get; set; }

  }

}
