using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderAO
  {

    [ScriptingProperty( "intensity" )]
    public Single Intensity { get; set; }

    [ScriptingProperty( "occlusionAmount" )]
    public Single OcclusionAmount { get; set; }

    [ScriptingProperty( "vertexAmbientOcclusion" )]
    public Boolean VertexAmbientOcclusion { get; set; }

  }

}
