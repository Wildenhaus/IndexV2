using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Materials
{

  public class MaterialLayer
  {

    [ScriptingProperty( "texName" )]
    public string TextureName { get; set; }

    [ScriptingProperty( "mtlName" )]
    public string MaterialName { get; set; }

    [ScriptingProperty( "tint" )]
    public float[] Tint { get; set; }

    [ScriptingProperty( "vcSet" )]
    public int VcSet { get; set; }

    [ScriptingProperty( "tilingU" )]
    public float TilingU { get; set; }

    [ScriptingProperty( "tilingV" )]
    public float TilingV { get; set; }

    [ScriptingProperty( "blending" )]
    public MaterialBlending Blending { get; set; }

    [ScriptingProperty( "uvSetIdx" )]
    public int UvSetIndex { get; set; }

  }

}
