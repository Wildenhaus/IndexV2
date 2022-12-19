using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderChameleon
  {

    [ScriptingProperty( "color0" )]
    public SaberColor Color0 { get; set; }

    [ScriptingProperty( "color1" )]
    public SaberColor Color1 { get; set; }

    [ScriptingProperty( "color2" )]
    public SaberColor Color2 { get; set; }

    [ScriptingProperty( "color3" )]
    public SaberColor Color3 { get; set; }

    [ScriptingProperty( "enabled" )]
    public Boolean Enabled { get; set; }

    [ScriptingProperty( "fresnelPwr" )]
    public Single FresnelPwr { get; set; }

    [ScriptingProperty( "offset1" )]
    public Single Offset1 { get; set; }

    [ScriptingProperty( "offset2" )]
    public Single Offset2 { get; set; }

  }

}
