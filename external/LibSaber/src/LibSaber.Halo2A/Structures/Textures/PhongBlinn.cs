using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{
    public class PhongBlinn
  {

    [ScriptingProperty( "multiplier" )]
    public Single Multiplier { get; set; }

    [ScriptingProperty( "normalScale" )]
    public Single NormalScale { get; set; }

    [ScriptingProperty( "power" )]
    public Single Power { get; set; }

    [ScriptingProperty( "spotDepth" )]
    public Single SpotDepth { get; set; }

  }

}
