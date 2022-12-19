using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderFoamDistribuance
  {

    [ScriptingProperty( "amplitude" )]
    public Single Amplitude { get; set; }

    [ScriptingProperty( "freq" )]
    public Single Frequency { get; set; }

    [ScriptingProperty( "speed" )]
    public Single Speed { get; set; }

  }

}
