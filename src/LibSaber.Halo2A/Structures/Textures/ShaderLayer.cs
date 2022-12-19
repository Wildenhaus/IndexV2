using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderLayer : ITextureNameProvider
  {

    [ScriptingProperty( "add_bloom" )]
    public Single AddBloom { get; set; }

    [ScriptingProperty( "alphaSource" )]
    public String AlphaSource { get; set; }

    [ScriptingProperty( "blendMode" )]
    public String BlendMode { get; set; }

    [ScriptingProperty( "embossment_depth" )]
    public Single EmbossmentDepth { get; set; }

    [ScriptingProperty( "emissive" )]
    public Single Emissive { get; set; }

    [ScriptingProperty( "enable_embossment" )]
    public Boolean EnableEmbossment { get; set; }

    [ScriptingProperty( "fake_light_contrast" )]
    public Single FakeLightContrast { get; set; }

    [ScriptingProperty( "specular_intensity" )]
    public Single SpecularIntensity { get; set; }

    [ScriptingProperty( "specular_power" )]
    public Single SpecularPower { get; set; }

    [ScriptingProperty( "tex_diffuse" )]
    public String TextureDiffuse { get; set; }

    [ScriptingProperty( "tex_normalmap" )]
    public String TextureNormalMap { get; set; }

    [ScriptingProperty( "texture" )]
    public String Texture { get; set; }

    [ScriptingProperty( "tint" )]
    public SaberColor Tint { get; set; }

    [ScriptingProperty( "uv_scale_x" )]
    public Single UvScaleX { get; set; }

    [ScriptingProperty( "uv_scale_y" )]
    public Single UvScaleY { get; set; }

    [ScriptingProperty( "uv_scroll_speed_x" )]
    public Single UvScrollSpeedX { get; set; }

    [ScriptingProperty( "uv_scroll_speed_y" )]
    public Single UvScrollSpeedY { get; set; }

    [ScriptingProperty( "uv_source" )]
    public String UvSource { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( !string.IsNullOrWhiteSpace( AlphaSource ) )
        yield return AlphaSource;

      if ( !string.IsNullOrWhiteSpace( TextureDiffuse ) )
        yield return TextureDiffuse;

      if ( !string.IsNullOrWhiteSpace( TextureNormalMap ) )
        yield return TextureNormalMap;

      if ( !string.IsNullOrWhiteSpace( Texture ) )
        yield return Texture;
    }
  }

}
