using LibSaber.Halo2A.Serialization;
using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class TextureDefinition : ITextureNameProvider
  {

    [ScriptingProperty( "__type_id" )]
    public String TypeId { get; set; }

    [ScriptingProperty( "convert_settings" )]
    public TextureConvertSettings ConvertSettings { get; set; }

    [ScriptingProperty( "description" )]
    public String Description { get; set; }

    [ScriptingProperty( "game_material" )]
    public String GameMaterial { get; set; }

    [ScriptingProperty( "isUltraHiRes" )]
    public Boolean IsUltraHiRes { get; set; }

    [ScriptingProperty( "mapping" )]
    public TextureMapping Mapping { get; set; }

    [ScriptingProperty( "materials" )]
    public Dictionary<string, TextureMaterial> Materials { get; set; }

    [ScriptingProperty( "rendering" )]
    public TextureRendering Rendering { get; set; }

    [ScriptingProperty( "shaders" )]
    public Dictionary<string, TextureShader> Shaders { get; set; }

    [ScriptingProperty( "strm_no_lowres" )]
    public Boolean StreamDisableLowRes { get; set; }

    [ScriptingProperty( "strm_priority" )]
    public String StreamPriority { get; set; }

    [ScriptingProperty( "strm_priority_cine" )]
    public String StreamPriorityCinematic { get; set; }

    [ScriptingProperty( "tags" )]
    public String[] Tags { get; set; }

    [ScriptingProperty( "usage" )]
    public String Usage { get; set; }

    [ScriptingProperty( "version" )]
    public Single Version { get; set; }

    public IEnumerable<string> GetTextureNames()
    {
      if ( !string.IsNullOrWhiteSpace( GameMaterial ) )
        yield return GameMaterial;

      if ( Materials != null )
      {
        foreach ( var material in Materials )
        {
          yield return material.Key;
          foreach ( var materialName in material.Value.GetTextureNames() )
            yield return materialName;
        }
      }

      if ( Shaders != null )
        foreach ( var shader in Shaders.Values )
          foreach ( var textureName in shader.GetTextureNames() )
            yield return textureName;
    }

  }

}
