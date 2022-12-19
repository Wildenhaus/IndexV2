using LibSaber.Halo2A.Serialization.Scripting;

namespace LibSaber.Halo2A.Structures.Textures
{

  public class TextureMapping
  {

    [ScriptingProperty( "address_u" )]
    public String AddressU { get; set; }

    [ScriptingProperty( "address_v" )]
    public String AddressV { get; set; }

    [ScriptingProperty( "anisotropy" )]
    public Single Anisotropy { get; set; }

    [ScriptingProperty( "lod_bias" )]
    public Single LodBias { get; set; }

  }

}
