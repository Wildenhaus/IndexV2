using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderPerturbation : ITextureNameProvider
  {

    [ScriptingProperty( "enabled" )]
    public Boolean Enabled { get; set; }

    [ScriptingProperty( "first" )]
    public ShaderPerturbationLayer FirstLayer { get; set; }

    [ScriptingProperty( "second" )]
    public ShaderPerturbationLayer SecondLayer { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( FirstLayer != null )
        foreach ( var textureName in FirstLayer.GetTextureNames() )
          yield return textureName;

      if ( SecondLayer != null )
        foreach ( var textureName in SecondLayer.GetTextureNames() )
          yield return textureName;
    }
  }

}
