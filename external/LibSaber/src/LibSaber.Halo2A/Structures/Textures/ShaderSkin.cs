using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderSkin : ITextureNameProvider
  {

    [ScriptingProperty( "detail" )]
    public ShaderDetailMap Detail { get; set; }

    [ScriptingProperty( "wrinkles" )]
    public ShaderSkinWrinkles Wrinkles { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( Detail != null )
        foreach ( var textureName in Detail.GetTextureNames() )
          yield return textureName;

      if ( Wrinkles != null )
        foreach ( var textureName in Wrinkles.GetTextureNames() )
          yield return textureName;
    }
  }

}
