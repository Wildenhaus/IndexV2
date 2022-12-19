using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class TextureConvertSettings
  {

    [ScriptingProperty( "color" )]
    public SaberColor Color { get; set; }

    [ScriptingProperty( "fade_begin" )]
    public Int32 FadeBegin { get; set; }

    [ScriptingProperty( "fade_end" )]
    public Int32 FadeEnd { get; set; }

    [ScriptingProperty( "fade_flag" )]
    public Boolean FadeFlag { get; set; }

    [ScriptingProperty( "filter" )]
    public Int32 Filter { get; set; }

    [ScriptingProperty( "format_descr" )]
    public String FormatDescription { get; set; }

    [ScriptingProperty( "format_name" )]
    public String FormatName { get; set; }

    [ScriptingProperty( "fp16" )]
    public Boolean FP16 { get; set; }

    [ScriptingProperty( "left_handed" )]
    public Boolean LeftHanded { get; set; }

    [ScriptingProperty( "mipmap_level" )]
    public Int32 MipMapLevel { get; set; }

    [ScriptingProperty( "resample" )]
    public Int32 Resample { get; set; }

    [ScriptingProperty( "sharpen" )]
    public Int32 Sharpen { get; set; }

    [ScriptingProperty( "sharpness" )]
    public Boolean Sharpness { get; set; }

    [ScriptingProperty( "uncompressed_flag" )]
    public Boolean UncompressedFlag { get; set; }

  }

}
