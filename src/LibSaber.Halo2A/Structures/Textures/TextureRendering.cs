using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class TextureRendering
  {

    [ScriptingProperty( "akill_ref" )]
    public Int32 AlphaKillRef { get; set; }

    [ScriptingProperty( "decay" )]
    public TextureDecay Decay { get; set; }

    [ScriptingProperty( "detail_density" )]
    public Single DetailDensity { get; set; }

    [ScriptingProperty( "detail_scale" )]
    public Single DetailScale { get; set; }

    [ScriptingProperty( "hdr_scale" )]
    public Single HdrScale { get; set; }

    [ScriptingProperty( "hm_range" )]
    public Single HeightMapRange { get; set; }

    [ScriptingProperty( "linear_rgb" )]
    public Boolean LinearRGB { get; set; }

    [ScriptingProperty( "sm_hi" )]
    public Boolean SmHi { get; set; }

    [ScriptingProperty( "tex_size_u" )]
    public Single TextureSizeU { get; set; }

    [ScriptingProperty( "tex_size_v" )]
    public Single TextureSizeV { get; set; }

    [ScriptingProperty( "use_akill" )]
    public Boolean UseAlphaKill { get; set; }

  }

}
