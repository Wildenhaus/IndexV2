using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Materials
{

  public class MaterialExtraParams
  {

    [ScriptingProperty( "reliefNormalmaps" )]
    public MaterialReliefNormalMaps ReliefNormalMaps { get; set; }

    [ScriptingProperty( "auxiliaryTextures" )]
    public MaterialAuxiliaryTextures AuxiliaryTextures { get; set; }

    [ScriptingProperty( "transparency" )]
    public MaterialTransparency Transparency { get; set; }

    [ScriptingProperty( "extraVertexColorData" )]
    public MaterialExtraVertexColorData ExtraVertexColorData { get; set; }

  }

}
