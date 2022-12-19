using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderAnimation : ITextureNameProvider
  {

    [ScriptingProperty( "bending" )]
    public ShaderAnimationBending Bending { get; set; }

    [ScriptingProperty( "blink" )]
    public ShaderGradient Blink { get; set; }

    [ScriptingProperty( "enable" )]
    public Boolean Enable { get; set; }

    [ScriptingProperty( "gradient" )]
    public ShaderGradient Gradient { get; set; }

    [ScriptingProperty( "noise" )]
    public AnimationNoise Noise { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( Blink != null )
        foreach ( var textureName in Blink.GetTextureNames() )
          yield return textureName;

      if ( Gradient != null )
        foreach ( var textureName in Gradient.GetTextureNames() )
          yield return textureName;
    }
  }

}
