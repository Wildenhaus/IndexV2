using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderDetailMap : ITextureNameProvider
  {

    [ScriptingProperty( "density" )]
    public Single Sensity { get; set; }

    [ScriptingProperty( "diffuse_tex" )]
    public String DiffuseTexture { get; set; }

    [ScriptingProperty( "scale" )]
    public Single Scale { get; set; }

    [ScriptingProperty( "tex" )]
    public String Texture { get; set; }

    [ScriptingProperty( "useDetailMask" )]
    public Boolean UseDetailMask { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( !string.IsNullOrWhiteSpace( DiffuseTexture ) )
        yield return DiffuseTexture;

      if ( !string.IsNullOrWhiteSpace( Texture ) )
        yield return Texture;
    }
  }

}
