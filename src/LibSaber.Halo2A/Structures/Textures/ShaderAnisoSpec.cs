using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderAnisoSpec
  {

    [ScriptingProperty( "angle" )]
    public Single Angle { get; set; }

    [ScriptingProperty( "enabled" )]
    public Boolean Enabled { get; set; }

    [ScriptingProperty( "extra" )]
    public ShaderAnisoSpecExtra Extra { get; set; }

    [ScriptingProperty( "intensity" )]
    public Single Intensity { get; set; }

    [ScriptingProperty( "tint" )]
    public SaberColor Tint { get; set; }

  }

}
