using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderSkinWrinkles : ITextureNameProvider
  {

    [ScriptingProperty( "enable" )]
    public Boolean Enable { get; set; }

    [ScriptingProperty( "mask0_3" )]
    public String Mask0_3 { get; set; }

    [ScriptingProperty( "mask4_7" )]
    public String Mask4_7 { get; set; }

    [ScriptingProperty( "mask8_11" )]
    public String Mask8_11 { get; set; }

    [ScriptingProperty( "mask12_15" )]
    public String Mask12_15 { get; set; }

    [ScriptingProperty( "normalmap" )]
    public String NormalMap { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( !string.IsNullOrWhiteSpace( Mask0_3 ) )
        yield return Mask0_3;

      if ( !string.IsNullOrWhiteSpace( Mask4_7 ) )
        yield return Mask4_7;

      if ( !string.IsNullOrWhiteSpace( Mask8_11 ) )
        yield return Mask8_11;

      if ( !string.IsNullOrWhiteSpace( Mask12_15 ) )
        yield return Mask12_15;

      if ( !string.IsNullOrWhiteSpace( NormalMap ) )
        yield return NormalMap;
    }
  }

}
