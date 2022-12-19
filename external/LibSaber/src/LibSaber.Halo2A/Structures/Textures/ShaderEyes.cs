using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderEyes
  {

    [ScriptingProperty( "glossiness" )]
    public Single Glossiness { get; set; }

    [ScriptingProperty( "intensity" )]
    public Single Intensity { get; set; }

    [ScriptingProperty( "nmScale" )]
    public Single NormalMapScale { get; set; }

    [ScriptingProperty( "x" )]
    public Single X { get; set; }

    [ScriptingProperty( "y" )]
    public Single Y { get; set; }

  }

}
