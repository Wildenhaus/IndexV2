using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderSoftZ
  {

    [ScriptingProperty( "enabled" )]
    public Boolean Enabled { get; set; }

    [ScriptingProperty( "inverted" )]
    public Boolean Inverted { get; set; }

    [ScriptingProperty( "nearEnabled" )]
    public Boolean NearEnabled { get; set; }

    [ScriptingProperty( "nearRange" )]
    public Single NearRange { get; set; }

    [ScriptingProperty( "range" )]
    public Single Range { get; set; }

  }

}
