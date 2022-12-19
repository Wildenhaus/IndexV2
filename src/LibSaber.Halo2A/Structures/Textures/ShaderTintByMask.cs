using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class ShaderTintByMask
  {

    [ScriptingProperty( "albedo" )]
    public SaberColor Albedo { get; set; }

    [ScriptingProperty( "carpaintMetallness" )]
    public SaberColor CarpaintMetallness { get; set; }

    [ScriptingProperty( "maskFromAlbedoAlpha" )]
    public Boolean MaskFromAlbedoAlpha { get; set; }

    [ScriptingProperty( "metallness" )]
    public SaberColor Metallness { get; set; }

  }

}
