using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class TextureShader : ITextureNameProvider
  {

    [ScriptingProperty( "__type_id" )]
    public String TypeId { get; set; }

    [ScriptingProperty( "advanced" )]
    public ShaderAdvanced Advanced { get; set; }

    [ScriptingProperty( "albedo_tint" )]
    public SaberColor AlbedoTint { get; set; }

    [ScriptingProperty( "alphaKill" )]
    public Boolean AlphaKillEnabled { get; set; }

    [ScriptingProperty( "alphaKillSmooth" )]
    public AlphaKillSmooth AlphaKillSmooth { get; set; }

    [ScriptingProperty( "ambient" )]
    public ShaderAO AmbientOcclusion { get; set; }

    [ScriptingProperty( "animation" )]
    public ShaderAnimation Animation { get; set; }

    [ScriptingProperty( "animationFrameBlend" )]
    public Boolean AnimationFrameBlendEnabled { get; set; }

    [ScriptingProperty( "animPeriod" )]
    public Single AnimationPeriod { get; set; }

    [ScriptingProperty( "anisotropic_spec" )]
    public ShaderAnisoSpec AnisotropicSpec { get; set; }

    [ScriptingProperty( "autogenPatches" )]
    public Boolean AutogenPatches { get; set; }

    [ScriptingProperty( "blendMode" )]
    public String BlendMode { get; set; }

    [ScriptingProperty( "carpaint" )]
    public ShaderCarPaint CarPaint { get; set; }

    [ScriptingProperty( "chameleon" )]
    public ShaderChameleon Chameleon { get; set; }

    [ScriptingProperty( "colors" )]
    public ShaderColors Colors { get; set; }

    [ScriptingProperty( "combiner" )]
    public String Combiner { get; set; }

    [ScriptingProperty( "depthPath" )]
    public Boolean DepthPath { get; set; }

    [ScriptingProperty( "detail" )]
    public ShaderDetailMap Detail { get; set; }

    [ScriptingProperty( "detailDiffuse" )]
    public ShaderDetailMap DetailDiffuse { get; set; }

    [ScriptingProperty( "diffuse" )]
    public ShaderDiffuse Diffuse { get; set; }

    [ScriptingProperty( "disable_fog" )]
    public Boolean DisableFog { get; set; }

    [ScriptingProperty( "distort" )]
    public ShaderDistortion Distort { get; set; }

    [ScriptingProperty( "distortBackground" )]
    public ShaderDistortionBackground DistortBackground { get; set; }

    [ScriptingProperty( "doubleSideLighting" )]
    public ShaderDoubleSideLighting DoubleSideLighting { get; set; }

    [ScriptingProperty( "emissive" )]
    public ShaderEmissive Emissive { get; set; }

    [ScriptingProperty( "eyes" )]
    public ShaderEyes Eyes { get; set; }

    [ScriptingProperty( "fadeDist" )]
    public Single FadeDist { get; set; }

    [ScriptingProperty( "fakeLight" )]
    public ShaderFakeLight FakeLight { get; set; }

    [ScriptingProperty( "foam" )]
    public ShaderFoam Foam { get; set; }

    [ScriptingProperty( "fresnel_intensity" )]
    public Single FresnelIntensity { get; set; }

    [ScriptingProperty( "fresnel_power" )]
    public Single FresnelPower { get; set; }

    [ScriptingProperty( "glass" )]
    public ShaderGlass Glass { get; set; }

    [ScriptingProperty( "glossiness" )]
    public ShaderGlossiness Glossiness { get; set; }

    [ScriptingProperty( "gradientBlink" )]
    public ShaderGradient GradientBlink { get; set; }

    [ScriptingProperty( "gradientScroll" )]
    public ShaderGradient GradientScroll { get; set; }

    [ScriptingProperty( "layer1" )]
    public ShaderLayer ShaderLayer1 { get; set; }

    [ScriptingProperty( "layer2" )]
    public ShaderLayer ShaderLayer2 { get; set; }

    [ScriptingProperty( "layerBase" )]
    public ShaderLayer ShaderLayerBase { get; set; }

    [ScriptingProperty( "layerSecond" )]
    public TextureLayer TextureLayerSecond { get; set; }

    [ScriptingProperty( "layerThird" )]
    public TextureLayer TextureLayerThird { get; set; }

    [ScriptingProperty( "maps" )]
    public ShaderMaps Maps { get; set; }

    [ScriptingProperty( "maskOffDistortion" )]
    public Boolean MaskOffDistortionEnabled { get; set; }

    [ScriptingProperty( "metalness" )]
    public SaberColor Metalness { get; set; }

    [ScriptingProperty( "nmScale" )]
    public Single NormalMapScale { get; set; }

    [ScriptingProperty( "no_backface_culling" )]
    public Boolean NoBackfaceCulling { get; set; }

    [ScriptingProperty( "no_ssao" )]
    public Boolean NoSsao { get; set; }

    [ScriptingProperty( "noCull" )]
    public Boolean NoCulling { get; set; }

    [ScriptingProperty( "noVertexColor" )]
    public Boolean NoVertexColor { get; set; }

    [ScriptingProperty( "overrideAffixScroll" )]
    public ShaderOverrideAffixScroll OverrideAffixScroll { get; set; }

    [ScriptingProperty( "parallax" )]
    public ShaderParallax Parallax { get; set; }

    [ScriptingProperty( "params" )]
    public ShaderParams ShaderParams { get; set; }

    [ScriptingProperty( "particle" )]
    public Boolean ParticleEnabled { get; set; }

    [ScriptingProperty( "perturbation" )]
    public ShaderPerturbation Perturbation { get; set; }

    [ScriptingProperty( "preset" )]
    public String Preset { get; set; }

    [ScriptingProperty( "reflection" )]
    public ShaderReflection Reflection { get; set; }

    [ScriptingProperty( "refraction" )]
    public ShaderRefraction Refraction { get; set; }

    [ScriptingProperty( "scale" )]
    public Single Scale { get; set; }

    [ScriptingProperty( "scrollSpeedScale" )]
    public Single ScrollSpeedScale { get; set; }

    [ScriptingProperty( "skin" )]
    public ShaderSkin Skin { get; set; }

    [ScriptingProperty( "softFreshnel" )]
    public ShaderSoftFresnel SoftFresnel { get; set; }

    [ScriptingProperty( "softZ" )]
    public ShaderSoftZ SoftZ { get; set; }

    [ScriptingProperty( "specular" )]
    public ShaderSpecular Specular { get; set; }

    [ScriptingProperty( "sprite" )]
    public Boolean Sprite { get; set; }

    [ScriptingProperty( "tex" )]
    public Dictionary<string, string> Textures { get; set; }

    [ScriptingProperty( "texNM" )]
    public String TextureNormalMap { get; set; }

    [ScriptingProperty( "tint" )]
    public SaberColor Tint { get; set; }

    [ScriptingProperty( "tintByMask" )]
    public ShaderTintByMask TintByMask { get; set; }

    [ScriptingProperty( "translucency" )]
    public ShaderTranslucency Translucency { get; set; }

    [ScriptingProperty( "useBillboards" )]
    public Boolean UseBillboards { get; set; }

    [ScriptingProperty( "useHDR" )]
    public Boolean UseHDR { get; set; }

    [ScriptingProperty( "writeDepth" )]
    public Boolean WriteDepth { get; set; }

    [ScriptingProperty( "z_func_less" )]
    public Boolean ZFuncLessEnabled { get; set; }

    [ScriptingProperty( "zTest" )]
    public Boolean ZTestEnabled { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( Animation != null )
        foreach ( var textureName in Animation.GetTextureNames() )
          yield return textureName;

      if ( Detail != null )
        foreach ( var textureName in Detail.GetTextureNames() )
          yield return textureName;

      if ( DetailDiffuse != null )
        foreach ( var textureName in DetailDiffuse.GetTextureNames() )
          yield return textureName;

      if ( Distort != null )
        foreach ( var textureName in Distort.GetTextureNames() )
          yield return textureName;

      if ( DistortBackground != null )
        foreach ( var textureName in DistortBackground.GetTextureNames() )
          yield return textureName;

      if ( Emissive != null )
        foreach ( var textureName in Emissive.GetTextureNames() )
          yield return textureName;

      if ( GradientBlink != null )
        foreach ( var textureName in GradientBlink.GetTextureNames() )
          yield return textureName;

      if ( GradientScroll != null )
        foreach ( var textureName in GradientScroll.GetTextureNames() )
          yield return textureName;

      if ( ShaderLayer1 != null )
        foreach ( var textureName in ShaderLayer1.GetTextureNames() )
          yield return textureName;

      if ( ShaderLayer2 != null )
        foreach ( var textureName in ShaderLayer2.GetTextureNames() )
          yield return textureName;

      if ( ShaderLayerBase != null )
        foreach ( var textureName in ShaderLayerBase.GetTextureNames() )
          yield return textureName;

      if ( TextureLayerSecond != null )
        foreach ( var textureName in TextureLayerSecond.GetTextureNames() )
          yield return textureName;

      if ( TextureLayerThird != null )
        foreach ( var textureName in TextureLayerThird.GetTextureNames() )
          yield return textureName;

      if ( Maps != null )
        foreach ( var textureName in Maps.GetTextureNames() )
          yield return textureName;

      if ( Perturbation != null )
        foreach ( var textureName in Perturbation.GetTextureNames() )
          yield return textureName;

      if ( Skin != null )
        foreach ( var textureName in Skin.GetTextureNames() )
          yield return textureName;

      if ( Textures != null )
        foreach ( var textureName in Textures.Values )
          yield return textureName;

      if ( !string.IsNullOrWhiteSpace( TextureNormalMap ) )
        yield return TextureNormalMap;
    }
  }

}
