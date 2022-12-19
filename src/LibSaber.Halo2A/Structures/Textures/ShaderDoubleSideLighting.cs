using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderDoubleSideLighting
  {

    [ScriptingProperty( "backLightTint" )]
    public SaberColor BackLightTint { get; set; }

    [ScriptingProperty( "use" )]
    public Boolean Use { get; set; }

    [ScriptingProperty( "viewDependence" )]
    public Single ViewDependence { get; set; }

  }

}
