using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderSpecular
  {

    [ScriptingProperty( "blinn" )]
    public PhongBlinn Blinn { get; set; }

    [ScriptingProperty( "phong" )]
    public PhongBlinn Phong { get; set; }

    [ScriptingProperty( "tint" )]
    public SaberColor Tint { get; set; }

  }

}
