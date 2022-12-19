using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Materials
{

  public class MaterialUpVector
  {

    [ScriptingProperty( "angle" )]
    public float Angle { get; set; }

    [ScriptingProperty( "enabled" )]
    public bool Enabled { get; set; }

    [ScriptingProperty( "falloff" )]
    public float Falloff { get; set; }

  }

}
