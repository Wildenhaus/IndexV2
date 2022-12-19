using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Materials
{

  public class MaterialTransparency
  {

    [ScriptingProperty( "colorSetIdx" )]
    public int ColorSetIndex { get; set; }

    [ScriptingProperty( "enabled" )]
    public int Enabled { get; set; }

    [ScriptingProperty( "multiplier" )]
    public float Multiplier { get; set; }

    [ScriptingProperty( "sources" )]
    public int Sources { get; set; }

  }

}
