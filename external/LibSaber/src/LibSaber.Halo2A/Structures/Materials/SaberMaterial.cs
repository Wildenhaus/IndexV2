using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Materials
{

  public class SaberMaterial
  {

    #region Properties

    [ScriptingProperty( "version" )]
    public int Version { get; set; }

    [ScriptingProperty( "shadingMtl_Tex" )]
    public string ShadingMaterialTexture { get; set; }

    [ScriptingProperty( "shadingMtl_Mtl" )]
    public string ShadingMaterialMaterial { get; set; }

    [ScriptingProperty( "lm" )]
    public MaterialLightMap LM { get; set; }

    [ScriptingProperty( "layer0" )]
    public MaterialLayer Layer0 { get; set; }

    [ScriptingProperty( "layer1" )]
    public MaterialLayer Layer1 { get; set; }

    [ScriptingProperty( "layer2" )]
    public MaterialLayer Layer2 { get; set; }

    [ScriptingProperty( "layer3" )]
    public MaterialLayer Layer3 { get; set; }

    [ScriptingProperty( "extraParams" )]
    public MaterialExtraParams ExtraParams { get; set; }

    public string MaterialName
    {
      get => $"{ShadingMaterialMaterial}_{ShadingMaterialTexture}";
    }

    #endregion

  }

}
