using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderSimulateShore
  {

    [ScriptingProperty( "enable" )]
    public Boolean Enable { get; set; }

    [ScriptingProperty( "foam" )]
    public ShaderFoam Foam { get; set; }

    [ScriptingProperty( "level" )]
    public Single Level { get; set; }

    [ScriptingProperty( "wave" )]
    public ShaderFoamWave Wave { get; set; }

  }

}
