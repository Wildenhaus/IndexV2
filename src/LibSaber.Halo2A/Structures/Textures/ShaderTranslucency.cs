using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderTranslucency
  {

    [ScriptingProperty( "blurWidth" )]
    public Single BlurWidth { get; set; }

    [ScriptingProperty( "firstColor" )]
    public SaberColor FirstColor { get; set; }

    [ScriptingProperty( "intensity" )]
    public Single Intensity { get; set; }

    [ScriptingProperty( "secondColor" )]
    public SaberColor SecondColor { get; set; }

    [ScriptingProperty( "width" )]
    public Single Width { get; set; }

  }

}
