using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderParams
  {

    [ScriptingProperty( "heightScale" )]
    public Single HeightScale { get; set; }

    [ScriptingProperty( "layerMultipliers" )]
    public ShaderLayerMultipliers LayerMultipliers { get; set; }

    [ScriptingProperty( "normalScale" )]
    public Single NormalScale { get; set; }

    [ScriptingProperty( "simulateFlow" )]
    public ShaderSimulateFlow SimulateFlow { get; set; }

    [ScriptingProperty( "simulateShore" )]
    public ShaderSimulateShore SimulateShore { get; set; }

    [ScriptingProperty( "simulateWind" )]
    public Boolean SimulateWind { get; set; }

    [ScriptingProperty( "speed" )]
    public Single Speed { get; set; }

    [ScriptingProperty( "tilingScale" )]
    public Single TilingScale { get; set; }

    [ScriptingProperty( "wavesMaxDistance" )]
    public Single WavesMaxDistance { get; set; }

  }

}
