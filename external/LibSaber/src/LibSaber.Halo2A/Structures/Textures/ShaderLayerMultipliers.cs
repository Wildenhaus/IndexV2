using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderLayerMultipliers
  {

    [ScriptingProperty( "layerScaleAtten" )]
    public Single LayerScaleAttenuation { get; set; }

    [ScriptingProperty( "layerWaveAtten" )]
    public Single LayerWaveAttenuation { get; set; }

  }

}
