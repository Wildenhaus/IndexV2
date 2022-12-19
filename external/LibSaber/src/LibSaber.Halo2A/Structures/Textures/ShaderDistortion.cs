using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderDistortion : ITextureNameProvider
  {

    [ScriptingProperty( "distortionScale" )]
    public Single DistortionScale { get; set; }

    [ScriptingProperty( "distortTexture" )]
    public String DistortTexture { get; set; }

    [ScriptingProperty( "distortTextureRotation" )]
    public Single DistortTextureRotation { get; set; }

    [ScriptingProperty( "distortTextureScaleX" )]
    public Single DistortTextureScaleX { get; set; }

    [ScriptingProperty( "distortTextureScaleY" )]
    public Single DistortTextureScaleY { get; set; }

    [ScriptingProperty( "speedScale" )]
    public Single SpeedScale { get; set; }

    [ScriptingProperty( "useWCS" )]
    public Boolean UseWCS { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( !string.IsNullOrWhiteSpace( DistortTexture ) )
        yield return DistortTexture;
    }
  }

}
