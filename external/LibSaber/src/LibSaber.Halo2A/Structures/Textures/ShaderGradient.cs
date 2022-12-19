using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderGradient : ITextureNameProvider
  {

    [ScriptingProperty( "speed" )]
    public Single Speed { get; set; }

    [ScriptingProperty( "tex" )]
    public String Texture { get; set; }

    [ScriptingProperty( "texPhaseOffset" )]
    public String TexturePhaseOffset { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( !string.IsNullOrWhiteSpace( Texture ) )
        yield return Texture;
    }
  }

}
