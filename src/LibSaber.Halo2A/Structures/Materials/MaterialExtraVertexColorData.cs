using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Materials
{

  public class MaterialExtraVertexColorData
  {

    [ScriptingProperty( "colorA" )]
    public MaterialColor ColorA { get; set; }

    [ScriptingProperty( "colorB" )]
    public MaterialColor ColorB { get; set; }

    [ScriptingProperty( "colorG" )]
    public MaterialColor ColorG { get; set; }

    [ScriptingProperty( "colorR" )]
    public MaterialColor ColorR { get; set; }

  }

}
