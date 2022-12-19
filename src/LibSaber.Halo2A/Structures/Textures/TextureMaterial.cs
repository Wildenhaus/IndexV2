using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class TextureMaterial : ITextureNameProvider
  {

    [ScriptingProperty( "__type_id" )]
    public String TypeId { get; set; }

    [ScriptingProperty( "default" )]
    public TextureMaterial DefaultMaterial { get; set; }

    [ScriptingProperty( "game_material" )]
    public String GameMaterial { get; set; }

    [ScriptingProperty( "preset" )]
    public String Preset { get; set; }

    [ScriptingProperty( "shaders" )]
    public Dictionary<string, TextureShader> Shaders { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( DefaultMaterial != null )
        foreach ( var textureName in DefaultMaterial.GetTextureNames() )
          yield return textureName;

      if ( !string.IsNullOrWhiteSpace( GameMaterial ) )
        yield return GameMaterial;

      if ( Shaders != null )
        foreach ( var shader in Shaders.Values )
          foreach ( var textureName in shader.GetTextureNames() )
            yield return textureName;
    }

  }

}
