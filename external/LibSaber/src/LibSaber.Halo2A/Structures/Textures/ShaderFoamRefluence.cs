using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderFoamRefluence
  {

    [ScriptingProperty( "amplitude" )]
    public Single Amplitude { get; set; }

    [ScriptingProperty( "freq" )]
    public Single Frequency { get; set; }

    [ScriptingProperty( "posStart" )]
    public Single PositionStart { get; set; }

  }

}
