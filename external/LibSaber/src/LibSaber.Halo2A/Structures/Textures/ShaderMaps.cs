using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderMaps : ITextureNameProvider
  {

    [ScriptingProperty( "caustics" )]
    public String Caustics { get; set; }

    [ScriptingProperty( "debris" )]
    public String Debris { get; set; }

    [ScriptingProperty( "detail" )]
    public ShaderDetailMap Detail { get; set; }

    [ScriptingProperty( "detail_nm" )]
    public ShaderDetailMap DetailNormalMap { get; set; }

    [ScriptingProperty( "foam" )]
    public String Foam { get; set; }

    [ScriptingProperty( "globalMask" )]
    public String GlobalMask { get; set; }

    [ScriptingProperty( "shoreDataMap" )]
    public String ShoreDataMap { get; set; }

    [ScriptingProperty( "shoreMask" )]
    public String ShoreMask { get; set; }

    [ScriptingProperty( "tintMask" )]
    public String TintMask { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( !string.IsNullOrWhiteSpace( Caustics ) )
        yield return Caustics;

      if ( !string.IsNullOrWhiteSpace( Debris ) )
        yield return Debris;

      if ( Detail != null )
        foreach ( var textureName in Detail.GetTextureNames() )
          yield return textureName;

      if ( DetailNormalMap != null )
        foreach ( var textureName in DetailNormalMap.GetTextureNames() )
          yield return textureName;

      if ( !string.IsNullOrWhiteSpace( Foam ) )
        yield return Foam;

      if ( !string.IsNullOrWhiteSpace( GlobalMask ) )
        yield return GlobalMask;

      if ( !string.IsNullOrWhiteSpace( ShoreDataMap ) )
        yield return ShoreDataMap;

      if ( !string.IsNullOrWhiteSpace( ShoreMask ) )
        yield return ShoreMask;

      if ( !string.IsNullOrWhiteSpace( TintMask ) )
        yield return TintMask;
    }
  }

}
